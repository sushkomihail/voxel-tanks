using System;
using Armor;
using ShootingSystems;
using Tank.Data;
using UnityEngine;
using Utils;

namespace Tank
{
    public class TankGun : MonoBehaviour
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private ShootingSystem _shootingSystem;
        [SerializeField] private int _projectileTrajectoryPredictionIterations = 50;
        [SerializeField] private float _projectileTrajectoryPredictionInterval = 0.1f;

        public ShootingSystem ShootingSystem => _shootingSystem;

        private const float MaxAimDistance = 50f;

        private GunData _data;
        private LaggedRotator _rotator;

        public void Initialize(GunData data)
        {
            _data = data;
            _rotator = new LaggedRotator(transform);
            _shootingSystem.Initialize();
        }

        public Vector3 PredictHitPoint(out Transform hitTransform, out Vector3 hitNormal, out Vector3 hitDirection)
        {
            hitTransform = null;
            hitNormal = Vector3.zero;
            hitDirection = Vector3.zero;
            
            Vector3 startPosition = transform.position;
            Vector3 velocity = transform.forward * _shootingSystem.GetProjectileSpeed();
            Vector3 previousPosition = startPosition;

            for (int i = 1; i < _projectileTrajectoryPredictionIterations; i++)
            {
                float time = i * _projectileTrajectoryPredictionInterval;
                Vector3 nextPosition = startPosition + velocity * time + Physics.gravity * (time * time) / 2;

                if (Physics.Linecast(previousPosition, nextPosition, out RaycastHit hit, _data.AimMask.value))
                {
                    hitTransform = hit.collider.transform;
                    hitNormal = hit.normal;
                    hitDirection = nextPosition - previousPosition;
                    return hit.point;
                }

                previousPosition = nextPosition;

                if (Vector3.Distance(startPosition, nextPosition) >= MaxAimDistance)
                {
                    break;
                }
            }
            
            return previousPosition;
        }

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 launchDirection = GetLaunchDirection(lookPosition);
            Vector3 targetDirection = Vector3.ProjectOnPlane(launchDirection, _turret.right);
            Vector3 upwards = Vector3.Cross(targetDirection, transform.right);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, upwards);
            _rotator.Rotate(targetRotation, _data.RotationSpeed, _data.RotationLag);
            ClampAngles();
        }
        
        public void HandleShooting()
        {
            _shootingSystem.Shoot();
        }
        
        private Vector3 GetLaunchDirection(Vector3 targetPosition, bool highArc = false)
        {
            Vector3 directionToTarget = targetPosition - transform.position;
            Vector3 directionToTargetXZ = new Vector3(directionToTarget.x, 0f, directionToTarget.z);
            float x = directionToTargetXZ.magnitude;
            float y = directionToTarget.y;

            float v = _shootingSystem.GetProjectileSpeed();
            float g = Mathf.Abs(Physics.gravity.y);

            float k = g * x * x / (2 * v * v);
            float b = -x;
            float c = y + k;
            float discriminant = b * b - 4 * k * c;
            
            float sqrtD = Mathf.Sqrt(discriminant);
            float angle;

            if (discriminant >= 0)
            {
                float tanTheta = highArc ? (-b + sqrtD) / (2 * k) : (-b - sqrtD) / (2 * k);
                angle = Mathf.Atan(tanTheta);
            }
            else
            {
                angle = Mathf.Atan(x / (x + Mathf.Sqrt(x * x + y * y))) * 0.5f + Mathf.PI / 4;
            }

            Vector3 launchDirection = directionToTargetXZ.normalized * Mathf.Cos(angle);
            launchDirection.y = Mathf.Sin(angle);
            return launchDirection;
        }

        private void ClampAngles()
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.x = localAngles.x > 180 ? localAngles.x - 360 : localAngles.x;
            localAngles.x = Mathf.Clamp(localAngles.x, -_data.MaxVerticalAngle, -_data.MinVerticalAngle);
            transform.localEulerAngles = localAngles;
        }
    }
}