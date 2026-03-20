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
            transform.LookAt(_camera.transform);
        }
    }
}