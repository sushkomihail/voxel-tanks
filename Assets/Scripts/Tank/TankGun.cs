using ShootingSystems;
using UnityEngine;

namespace Tank
{
    public class TankGun : MonoBehaviour
    {
        [SerializeField] private TankCamera _camera;
        [SerializeField] private Transform _turret;
        [SerializeField] private float _minVerticalAngle = -5f;
        [SerializeField] private float _maxVerticalAngle = 20f;
        [SerializeField] private float _caliber = 100f;
        [SerializeField] private ShootingSystem _shootingSystem;

        private const float RotationSpeed = 50f;
        
        private float _elapsedReloadingTime;
        
        public void Init()
        {
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
            Vector3 projectedDirectionToTarget = 
                Vector3.ProjectOnPlane(targetPosition - transform.position, _turret.right);
            
            Vector3 xzDirectionProjection = 
                new Vector3(projectedDirectionToTarget.x, 0, projectedDirectionToTarget.z);
            
            float x = xzDirectionProjection.magnitude;
            float y = projectedDirectionToTarget.y;

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
            
            Vector3 shootingDirection = xzDirectionProjection.normalized * (v * Mathf.Cos(launchAngle)) + 
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
        
        public void Shoot()
        {
            _shootingSystem.Shoot();
        }
    }
}