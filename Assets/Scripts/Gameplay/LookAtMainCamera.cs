using UnityEngine;

namespace Gameplay
{
    internal sealed class LookAtMainCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        private void LateUpdate()
        {
            if (MainCamera != null)
            {
                transform.LookAt(MainCamera.transform);
            }
        }
    }
}
