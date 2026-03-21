using ShootingSystems;
using Tank.Camera;
using UnityEngine;

namespace Tank
{
    public class TankGun : MonoBehaviour
    {
        [SerializeField] private TankCamera _camera;
        [SerializeField] private Transform _turret;
        [SerializeField] private float _minVerticalAngle = -5f;
        [SerializeField] private float _maxVerticalAngle = 20f;
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField] private float _caliber = 100f;
        [SerializeField] private ShootingSystem _shootingSystem;
        [SerializeField] private LayerMask _aimMask = ~(1 << 6);

        public ShootingSystem ShootingSystem => _shootingSystem;
        
        private const float MaxAimDistance = 100f;
        private float _elapsedReloadingTime;
        
        public void Init()
        {
            _shootingSystem.Init();
        }

        public Vector3 GetAimPoint()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, MaxAimDistance, _aimMask.value))
            {
                return hit.point;
            }

            return transform.position + transform.forward * MaxAimDistance;
        }

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, _turret.right);
            Vector3 upwards = Vector3.Cross(targetDirection, transform.right);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, upwards);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            ClampAngles();
        }
        
        public void Shoot()
        {
            _shootingSystem.Shoot();
        }

        private void ClampAngles()
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.x = localAngles.x > 180 ? localAngles.x - 360 : localAngles.x;
            localAngles.x = Mathf.Clamp(localAngles.x, -_maxVerticalAngle, -_minVerticalAngle);
            transform.localEulerAngles = localAngles;
        }
    }
}