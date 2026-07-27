using EquipmentSystem;
using InputSystem;
using Navigation;
using OutlineSystem;
using Tank.Data;
using Tank.Modules;
using Tank.View;
using UnityEngine;
using UpgradeSystem;
using VContainer;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankView))]
    public abstract class TankController : MonoBehaviour, IRouterTarget, IOutlineTrigger
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;

        protected TankView _view;
        
        private UpgradeManager _upgradeManager;
        private UpgradeBroker _upgradeBroker;
        
        public IInput Input { get; protected set; }
        public TankHealth Health { get; } = new();
        public Equipment Equipment { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsActive { get; private set; } = true;
        
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

            var tankModules = new TankModule[]
            {
                _chassis.LeftTrack,
                _chassis.RightTrack
            };
            Equipment = new Equipment(_data.Equipment, tankModules);
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TransmissionData, _data.TrackData, Health);
            _turret.Initialize(_data.TurretData, Health);
            _gun.Initialize(_data.GunData, Health, this);
        }

        private void OnDestroy()
        {
            Health.OnHealthChanged -= _view.UpdateHealthVisuals;
            Health.OnDeath -= OnDeath;
            
            _upgradeBroker.OnStatModifierChanged -= Health.QueryMaxHealth;
            
            Equipment.Dispose();
        }

        private void FixedUpdate()
        {
            Vector2 moveInputVector = Input.GetMoveInput();
            _chassis.HandleMovement(moveInputVector);
        }
        
        public void SetOutlineEnabled(bool enabled)
        {
            _view.SetOutlineEnabled(enabled);
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