using Input;
using ShootingSystems;
using UnityEngine;

namespace Tank
{
    public class TankGun : MonoBehaviour
    {
        [SerializeField] private TankCamera _camera;
        [SerializeField] private float _minVerticalAngle;
        [SerializeField] private float _maxVerticalAngle;
        [SerializeField] private float _caliber;
        [SerializeField] private ShootingSystem _shootingSystem;

        private const float RotationSpeed = 50f;
        
        private float _elapsedReloadingTime;
        
        public void Init()
        {
            // PlayerInput.Instance.GetShootAction().performed += _ => Shoot();
            _shootingSystem.Init();
        }

        public void OnUpdate()
        {
            Rotate();
            _shootingSystem.OnUpdate();
        }

        public void Rotate()
        {
            Vector3 targetPosition = _camera.CastRay();
            Vector3 targetDirection = GetShootingDirection(targetPosition);
            Vector3 upwards = Vector3.Cross(targetDirection, transform.right);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, upwards);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            ClampAngles();
        }

        private Vector3 GetShootingDirection(Vector3 targetPosition)
        {
            Vector3 directionToTarget = Vector3.ProjectOnPlane(targetPosition - transform.position, transform.right);
            Vector3 horizontalProjection = new Vector3(directionToTarget.x, 0, directionToTarget.z);
            float x = horizontalProjection.magnitude;
            float y = directionToTarget.y;

            float v = _shootingSystem.GetProjectileSpeed();
            float v2 = v * v;
            float v4 = v2 * v2;
            float x2 = x * x;
            float g = Physics.gravity.magnitude;
            
            float launchAngleTan;

            if (v4 - g * (g * x2 + 2 * y * v2) < 0)
            {
                launchAngleTan = 1;
            }
            else
            {
                launchAngleTan = Mathf.Min(
                    (v2 - Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2))) / (g * x), 
                    (v2 + Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2))) / (g * x));
            }

            float launchAngle = Mathf.Atan(launchAngleTan);
            Vector3 shootingDirection = horizontalProjection.normalized * (v * Mathf.Cos(launchAngle)) + 
                        Vector3.up * (v * Mathf.Sin(launchAngle));
            return shootingDirection;
        }

        private void ClampAngles()
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.x = localAngles.x > 180 ? localAngles.x - 360 : localAngles.x;
            localAngles.x = Mathf.Clamp(localAngles.x, -_maxVerticalAngle, -_minVerticalAngle);
            transform.localEulerAngles = localAngles;
        }
        
        private void Shoot()
        {
            _shootingSystem.Shoot();
        }
    }
}