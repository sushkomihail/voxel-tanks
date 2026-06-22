using Navigation;
using Tank.Data;
using Tank.View;
using UnityEngine;
using Input = InputSystem.Input;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankView))]
    public abstract class TankController : MonoBehaviour, IRouterTarget
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;
        
        public Input Input { get; protected set; }
        public TankHealth Health { get; } = new();
        public TankBattleData BattleData { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsActive { get; private set; } = true;

        protected TankView _view;

        public virtual void Initialize()
        {
            _view = GetComponent<TankView>();
            _view.Initialize();

            // Order is important
            Health.OnHealthChanged += _view.UpdateHealthVisuals;
            Health.OnDeath += OnDeath;
            Health.Initialize(_data.HealthData);
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TransmissionData, _data.TrackData, Health);
            _turret.Initialize(_data.TurretData, Health);
            _gun.Initialize(_data.GunData, Health);
            
            BattleData = new TankBattleData();
        }

        private void OnDestroy()
        {
            Health.OnHealthChanged -= _view.UpdateHealthVisuals;
            Health.OnDeath -= OnDeath;
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