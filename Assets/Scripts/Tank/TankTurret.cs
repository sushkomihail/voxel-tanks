using ArmorSystem;
using Tank.Data;
using UnityEngine;
using UpgradeSystem;
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
        private UpgradeBroker _upgradeBroker;
        private TankController _owner;

        private float RotationSpeed
        {
            get
            {
                StatQuery query = new StatQuery(StatType.TurretRotationSpeed, _data.RotationSpeed);
                _upgradeBroker.Query(_owner, query);
                return query.Value;
            }
        }
        
        [Inject]
        public void Construct(CollidersUpdater collidersUpdater, UpgradeBroker upgradeBroker)
        {
            _collidersUpdater = collidersUpdater;
            _upgradeBroker = upgradeBroker;
        }

        public void Initialize(TurretData data, TankHealth health)
        {
            _data = data;
            _rotator = new LaggedRotator(transform);

            if (transform.root.TryGetComponent(out TankController controller))
            {
                _owner = controller;
            }
            
            InitializeCollider(health);
        }

        public void Rotate(Vector3 lookPosition)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, transform.up);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            _rotator.Rotate(targetRotation, RotationSpeed, _data.RotationLag);            
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
        
        private void InitializeArmorAreas(Armor[] armorAreas, TankHealth health)
        {
            foreach (Armor armor in armorAreas)
            {
                armor.Initialize(health, _upgradeBroker, _owner);
            }
        }
    }
}