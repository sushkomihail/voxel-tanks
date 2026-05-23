using InputSystem;
using Tank.Data;
using UnityEngine;
using Input = InputSystem.Input;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankHealth), typeof(TankView))]
    public abstract class TankController : MonoBehaviour, IRouterTarget
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;
        
        public Input Input { get; protected set; }
        public TankHealth Health { get; private set; }
        public TankBattleData BattleData { get; private set; }
        
        protected TankView _view;

        protected void Initialize()
        {
            _view = GetComponent<TankView>();
            _view.Initialize();
            
            Health = GetComponent<TankHealth>();
            Health.OnHealthChanged += _view.OnHealthChanged;
            Health.OnDeath += OnDeath;
            Health.Initialize(_data.HealthData);
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TrackData);
            _turret.Initialize(_data.TurretData);
            _gun.Initialize(_data.GunData);
            
            BattleData = new TankBattleData();
        }

        private void OnDestroy()
        {
            Health.OnHealthChanged -= _view.OnHealthChanged;
            Health.OnDeath -= OnDeath;
        }

        private void FixedUpdate()
        {
            Vector2 moveInputVector = Input.GetMoveInput();
            _chassis.Move(moveInputVector);
            _chassis.Rotate(moveInputVector);
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
            Input.Disable();
            _view.OnDeath();
        }
    }
}