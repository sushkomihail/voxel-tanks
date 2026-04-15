using ShootingSystems;
using Tank.Data;
using UnityEngine;

namespace Tank
{
    public class TankGun : MonoBehaviour
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private ShootingSystem _shootingSystem;

        public ShootingSystem ShootingSystem => _shootingSystem;
        
        private GunData _data;
        private const float MaxAimDistance = 100f;
        private float _elapsedReloadingTime;
        
        public void Initialize(GunData data)
        {
            _data = data;
            _shootingSystem.Initialize();
        }

        public Vector3 GetAimPoint()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, MaxAimDistance, _data.AimMask.value))
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
                Quaternion.RotateTowards(transform.rotation, targetRotation, _data.RotationSpeed * Time.deltaTime);
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
            localAngles.x = Mathf.Clamp(localAngles.x, -_data.MaxVerticalAngle, -_data.MinVerticalAngle);
            transform.localEulerAngles = localAngles;
        }
    }
}