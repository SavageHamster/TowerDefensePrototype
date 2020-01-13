using System.Collections;
using UnityEngine;

namespace VFX
{
    internal abstract class VFXBase : MonoBehaviour
    {
        [SerializeField]
        private float _lifetime = 0.5f;
        [SerializeField]
        private ParticleSystem _particleSystem;

        private static WaitForSeconds _waitForLifetime;

        private Coroutine _releaseCoroutine;

        private void Awake()
        {
            _waitForLifetime = new WaitForSeconds(_lifetime);
        }

        private void OnEnable()
        {
            if (_releaseCoroutine != null)
            {
                StopCoroutine(_releaseCoroutine);
            }

            _releaseCoroutine = StartCoroutine(ReleaseAsync());
        }

        protected abstract void Release();

        private IEnumerator ReleaseAsync()
        {
            yield return _waitForLifetime;

            _particleSystem.Stop();
            Release();
        }
    }
}
