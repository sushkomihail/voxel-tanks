using UnityEngine;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            LookAtCamera();
        }

        private void LookAtCamera()
        {
            if (_camera)
            {
                Vector3 targetDirection = transform.position - _camera.transform.position;
                transform.rotation = Quaternion.LookRotation(targetDirection);
            }
        }
    }
}