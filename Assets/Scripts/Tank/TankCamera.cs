using OutlineSystem;
using UnityEngine;

namespace Tank
{
    public class TankCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _maxVerticalAngle = 25f;
        [SerializeField] private float _minVerticalAngle = -70f;
        [SerializeField] private float _cameraRadius = 0.1f;
        [SerializeField] private LayerMask _targetMask = (1 << 3) | (1 << 9) | (1 << 10) | (1 << 11);
        [SerializeField] private LayerMask _collisionMask = (1 << 3) | (1 << 9) | (1 << 10);
        
        public Camera Camera => _camera;

        private const float MaxRayDistance = 50f;
        
        private RaycastHit _hit;
        private ObjectHighlighter _highlighter;

        // TODO: Make sensitivity settings
        private const float Sensitivity = 20f;

        private void Awake()
        {
            _highlighter = new ObjectHighlighter();
        }

        private void Update()
        {
            _highlighter.TryHighlightFocusObject(_hit);
        }

        public void FollowTarget(Transform target, Vector3 offset)
        {
            transform.position = target.position;
            _camera.transform.localPosition = offset;
            HandleCollision(offset);
        }

        public void Rotate(Vector2 lookAxes)
        {
            Vector3 localAngles = transform.localEulerAngles;
            
            localAngles.y += lookAxes.x * Sensitivity * Time.deltaTime;
            
            localAngles.x -= lookAxes.y * Sensitivity * Time.deltaTime;
            localAngles.x = localAngles.x > 180 ? localAngles.x - 360 : localAngles.x;
            localAngles.x = Mathf.Clamp(localAngles.x, -_maxVerticalAngle, -_minVerticalAngle);
            
            transform.localEulerAngles = localAngles;
        }
        
        public Vector3 CastRay()
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            if (Physics.Raycast(ray, out _hit, MaxRayDistance, _targetMask.value, QueryTriggerInteraction.Ignore))
            {
                return _hit.point;
            }

            return _camera.transform.position + _camera.transform.forward * MaxRayDistance;
        }
        
        private void HandleCollision(Vector3 offset)
        {
            Vector3 rayDirection = _camera.transform.position - transform.position;
            float rayDistance = rayDirection.magnitude;
            Ray ray = new Ray(transform.position, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, _collisionMask.value, QueryTriggerInteraction.Ignore))
            {
                _camera.transform.localPosition = offset.normalized * (hit.distance - _cameraRadius);
            }
        }
    }
}