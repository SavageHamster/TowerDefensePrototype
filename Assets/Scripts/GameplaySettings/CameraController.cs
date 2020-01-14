using UnityEngine;

namespace Gameplay
{
    internal sealed class CameraController : MonoBehaviour
    {
        private const float MovementSpeed = 30f;
        private const float RotationSpeed = 100f;
        private const float ZoomByMouseSpeed = 1000f;

        [SerializeField]
        private Camera _camera;

        private void LateUpdate()
        {
            var isMouse1Pressed = Input.GetMouseButton(1);
            var isMouse2Pressed = Input.GetMouseButton(2);
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            var mouseWheel = Input.GetAxis("Mouse ScrollWheel");

            if (isMouse1Pressed)
            {
                RotateUp(mouseY);
                RotateRight(mouseX);
            }

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            MoveForward(vertical);
            MoveRight(horizontal);

            _camera.transform.position += _camera.transform.forward * mouseWheel * ZoomByMouseSpeed * Time.deltaTime;
        }

        private void RotateRight(float multiplier = 1)
        {
            _camera.transform.Rotate(Vector3.up, RotationSpeed * multiplier * Time.deltaTime, Space.World);
        }

        private void RotateUp(float multiplier = 1)
        {
            _camera.transform.Rotate(-Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized, RotationSpeed * multiplier * Time.deltaTime, Space.World);
        }

        private void MoveForward(float multiplier = 1)
        {
            _camera.transform.position += Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized * MovementSpeed * multiplier * Time.deltaTime;
        }

        private void MoveRight(float multiplier = 1)
        {
            _camera.transform.position += _camera.transform.right * MovementSpeed * multiplier * Time.deltaTime;
        }
    }
}
