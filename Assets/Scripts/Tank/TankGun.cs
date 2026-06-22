using ArmorSystem;
using ShootingSystems;
using Tank.Data;
using UnityEngine;
using Utils;
using VContainer;

namespace Tank
{
    public class TankGun : MonoBehaviour
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private Transform _collider;
        [SerializeField] private Transform _projectilePivot;
        [SerializeField] private ShootingSystem _shootingSystem;
        [SerializeField] private int _projectileTrajectoryPredictionIterations = 70;

        public ShootingSystem ShootingSystem => _shootingSystem;

        private GunData _data;
        private LaggedRotator _rotator;
        private CollidersUpdater _collidersUpdater;

        [Inject]
        public void Construct(CollidersUpdater collidersUpdater)
        {
            _collidersUpdater = collidersUpdater;
        }
        
        public void Initialize(GunData data, TankHealth health)
        {
            _data = data;
            _rotator = new LaggedRotator(transform);
            _shootingSystem.Initialize();
            
            InitializeCollider(health);
        }

        public Vector3 PredictHitPoint(out Transform hitTransform, out Vector3 hitNormal, out Vector3 hitDirection)
        {
            hitTransform = null;
            hitNormal = Vector3.zero;
            hitDirection = Vector3.zero;
    
            Vector3 currentPosition = _projectilePivot.position;
            Vector3 velocity = _projectilePivot.forward * _shootingSystem.GetProjectileSpeed();
    
            float flightDistance = 0f;

            for (int i = 0; i < _projectileTrajectoryPredictionIterations; i++)
            {
                velocity += Physics.gravity * Time.fixedDeltaTime;
                
                Vector3 moveDirection = velocity * Time.fixedDeltaTime;
                float moveDistance = moveDirection.magnitude;
        
                flightDistance += moveDistance;

                if (flightDistance > _shootingSystem.GetProjectileMaxFlightDistance())
                {
                    break;
                }

                Vector3 nextPosition = currentPosition + moveDirection;
                if (Physics.Linecast(currentPosition, nextPosition, out RaycastHit hit, _data.AimMask.value))
                {
                    hitTransform = hit.collider.transform;
                    hitNormal = hit.normal;
                    hitDirection = moveDirection.normalized;
                    return hit.point;
                }

                currentPosition = nextPosition;
            }
    
            return currentPosition;
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
            Vector3 directionToTarget = targetPosition - _projectilePivot.position;
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

        private void InitializeCollider(TankHealth health)
        {
            if (!_collider) return;
            
            Transform collider = Instantiate(_collider, transform.position, transform.rotation);

            var armorAreas = collider.GetComponentsInChildren<Armor>();
            InitializeArmorAreas(armorAreas, health);
            
            _collidersUpdater.AddCollider(collider, transform);
            collider.parent = _collidersUpdater.transform;
        }
        
        private static void InitializeArmorAreas(Armor[] armorAreas, TankHealth health)
        {
            foreach (Armor armor in armorAreas)
            {
                armor.Initialize(health);
            }
        }
    }
}