using Input;
using UnityEngine;

namespace Tank
{
    public class TankCamera : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _cameraTransform;

        private const float MaxRayDistance = 500;

        private readonly float _sensitivity = 50;

        public void Rotate()
        {
            Vector2 lookAxes = PlayerInput.Instance.GetLookInputVector();
            Vector3 localAngles = _pivot.localEulerAngles;
            localAngles.y += lookAxes.x * _sensitivity * Time.deltaTime;
            localAngles.x -= lookAxes.y * _sensitivity * Time.deltaTime;
            _pivot.localEulerAngles = localAngles;
        }
        
        public Vector3 CastRay()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance))
            {
                return hit.point;
            }

            return _cameraTransform.position + _cameraTransform.forward * MaxRayDistance;
        }
    }
}