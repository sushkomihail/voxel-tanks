using ArmorSystem;
using Tank.Data;
using UnityEngine;
using Utils;
using VContainer;

namespace Tank
{
    public class TankTurret : MonoBehaviour
    {
        [SerializeField] private Transform _collider;
        
        private TurretData _data;
        private LaggedRotator _rotator;
        private CollidersUpdater _collidersUpdater;
        
        [Inject]
        public void Construct(CollidersUpdater collidersUpdater)
        {
            _collidersUpdater = collidersUpdater;
        }

        public void Initialize(TurretData data, TankHealth health)
        {
            _data = data;
            _rotator = new LaggedRotator(transform);
            
            InitializeCollider(health);
        }

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            _rotator.Rotate(targetRotation, _data.RotationSpeed, _data.RotationLag);            
            ClampRotation();
        }

        private void ClampRotation()
        {
            Vector3 localAngles = transform.localEulerAngles;
            localAngles.y = localAngles.y > 180 ? localAngles.y - 360 : localAngles.y;
            localAngles.y = Mathf.Clamp(localAngles.y, -_data.MaxHorizontalAngle, -_data.MinHorizontalAngle);
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