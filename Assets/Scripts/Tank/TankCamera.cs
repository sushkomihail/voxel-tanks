using UnityEngine;

namespace Tank
{
    public class TankCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public Camera Camera => _camera;

        private const float MaxRayDistance = 500f;

        // TODO: Make sensitivity settings
        private const float Sensitivity = 20f;

        public void FollowTarget(Transform target, Vector3 offset)
        {
            transform.position = target.position;
            _camera.transform.localPosition = offset;
        }

        public void Rotate(Vector2 lookAxes)
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.y += lookAxes.x * Sensitivity * Time.deltaTime;
            localAngles.x -= lookAxes.y * Sensitivity * Time.deltaTime;
            transform.localEulerAngles = localAngles;
        }
        
        public Vector3 CastRay()
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            if (Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance))
            {
                return hit.point;
            }

            return _camera.transform.position + _camera.transform.forward * MaxRayDistance;
        }
    }
}