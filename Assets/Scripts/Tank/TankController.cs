using Input;
using Tank.Data;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankHealth), typeof(TankView))]
    public abstract class TankController : MonoBehaviour, IRouterTarget
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;
        
        public TankHealth Health { get; private set; }
        public TankBattleData BattleData { get; private set; }
        
        protected Input.Input _input;
        
        private TankView _view;

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
            Vector2 moveInputVector = _input.GetMoveInput();
            _chassis.Move(moveInputVector);
            _chassis.Rotate(moveInputVector);
        }

        protected void Shoot()
        {
            if (_input.GetShootInput())
            {
                _gun.HandleShooting();
            }
        }
        
        private void OnDeath()
        {
            _input.Disable();
            _view.OnDeath();
        }
    }
}