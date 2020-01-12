using UnityEngine;

namespace Gameplay
{
    internal sealed class Weapon : MonoBehaviour
    {
        private float _nextShotTime;
        private float _cooldownSec;

        private void Update()
        {
            if (Time.time >= _nextShotTime)
            {
                Shot();

                _nextShotTime = Time.time + _cooldownSec;
            }
        }

        private void Initialize(int shotsPerSec)
        {
            _cooldownSec = 1f / shotsPerSec;
        }

        private void Shot()
        {

        }
    }
}
