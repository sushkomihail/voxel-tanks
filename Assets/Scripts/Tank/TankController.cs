using System;
using EquipmentSystem;
using InputSystem;
using Navigation;
using OutlineSystem;
using Tank.Data;
using Tank.Modules;
using UnityEngine;
using UpgradeSystem;
using VContainer;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(AIInput), typeof(TankView))]
    public class TankController : MonoBehaviour, IRouterTarget, IOutlineTrigger
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private Vector3 _cameraFollowingOffset = new(0f, 3f, -9.2f);

        public event Action TankCameraFollowing;
        
        private GameInput _inputActions;
        private UpgradeManager _upgradeManager;
        private UpgradeBroker _upgradeBroker;
        
        public IInput Input { get; private set; }
        public TankChassis Chassis => _chassis;
        public TankGun Gun => _gun;
        public Transform CameraTarget => _cameraTarget;
        public Vector3 CameraFollowingOffset => _cameraFollowingOffset;
        public TankView View { get; private set; }
        public TankHealth Health { get; private set; }
        public Equipment Equipment { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsActive { get; private set; } = true;
        
        [Inject]
        public void Construct(
            GameInput inputActions,
            UpgradeManager upgradeManager,
            UpgradeBroker upgradeBroker)
        {
            _inputActions = inputActions;
            _upgradeManager = upgradeManager;
            _upgradeBroker = upgradeBroker;
        }

        public void Initialize(TankConfigurator configurator)
        {
            Input = configurator.GetInput();
            
            View = GetComponent<TankView>();
            View.Initialize();

            Health = new TankHealth(_data.HealthData, _upgradeManager, _upgradeBroker, this);
            Health.OnDeath += OnDeath;

            _upgradeBroker.OnStatModifierChanged += Health.QueryMaxHealth;

            var tankModules = new TankModule[]
            {
                _chassis.LeftTrack,
                _chassis.RightTrack
            };
            Equipment = new Equipment(_inputActions, _data.Equipment, tankModules);
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TransmissionData, _data.TrackData, Health);
            _turret.Initialize(_data.TurretData, Health);
            _gun.Initialize(_data.GunData, Health, this);
            
            configurator.Configure();
            
            Health.InvokeHealthChanged();
        }

        private void OnDestroy()
        {
            Health.OnHealthChanged -= View.UpdateOverTankHealthBar;
            Health.OnDeath -= OnDeath;
            
            _upgradeBroker.OnStatModifierChanged -= Health.QueryMaxHealth;
            
            TankCameraFollowing = null;
            
            Input.Destroy();
            Equipment.Dispose();
        }
        
        private void Update()
        {
            if (Input == null) return;
            
            Vector3 lookPosition = Input.GetLookPoint();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            Shoot();
        }

        private void FixedUpdate()
        {
            if (Input == null) return;
            
            Vector2 moveInputVector = Input.GetMoveInput();
            _chassis.HandleMovement(moveInputVector);
        }
        
        private void LateUpdate()
        {
            TankCameraFollowing?.Invoke();
        }

        public void SetCombatActionsEnabled(bool enabled)
        {
            if (enabled)
            {
                _inputActions.Tank.Look.Enable();
                _inputActions.Tank.Shoot.Enable();
            }
            else
            {
                _inputActions.Tank.Look.Disable();
                _inputActions.Tank.Shoot.Disable();
            }
        }
        
        public void SetOutlineEnabled(bool enabled)
        {
            View.SetOutlineEnabled(enabled);
        }

        private void Shoot()
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
            View.ShowDeathVisuals();
        }
    }
}