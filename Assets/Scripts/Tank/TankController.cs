using EquipmentSystem;
using InputSystem;
using Navigation;
using Tank.Data;
using Tank.View;
using UnityEngine;
using UpgradeSystem;
using VContainer;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankView))]
    public abstract class TankController : MonoBehaviour, IRouterTarget
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;
        
        public IInput Input { get; protected set; }
        public TankHealth Health { get; } = new();
        public Equipment Equipment { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsActive { get; private set; } = true;

        protected TankView _view;
        
        private UpgradeManager _upgradeManager;
        private UpgradeBroker _upgradeBroker;
        
        [Inject]
        public void Construct(UpgradeManager upgradeManager, UpgradeBroker upgradeBroker)
        {
            _upgradeManager = upgradeManager;
            _upgradeBroker = upgradeBroker;
        }

        public virtual void Initialize()
        {
            _view = GetComponent<TankView>();
            _view.Initialize();

            // Order is important
            Health.OnHealthChanged += _view.UpdateHealthVisuals;
            Health.OnDeath += OnDeath;
            Health.Initialize(_data.HealthData, _upgradeManager, _upgradeBroker, this);

            _upgradeBroker.OnStatModifierChanged += Health.QueryMaxHealth;

            Equipment = new Equipment(_data.EquipmentData);
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TransmissionData, _data.TrackData, Health);
            _turret.Initialize(_data.TurretData, Health);
            _gun.Initialize(_data.GunData, Health, this);
        }

        private void OnDestroy()
        {
            Health.OnHealthChanged -= _view.UpdateHealthVisuals;
            Health.OnDeath -= OnDeath;
            
            _upgradeBroker.OnStatModifierChanged -= Health.QueryMaxHealth;
        }

        private void FixedUpdate()
        {
            Vector2 moveInputVector = Input.GetMoveInput();
            _chassis.HandleMovement(moveInputVector);
        }

        protected void Shoot()
        {
            if (Input.GetShootInput())
            {
                _gun.HandleShooting();
            }
        }

        private void OnDeath()
        {
            IsActive = false;
            Input.Disable();
            _view.ShowDeathVisuals();
        }
    }
}