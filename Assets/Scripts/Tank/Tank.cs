using Input;
using Tank.Data;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankHealth), typeof(TankView))]
    public abstract class Tank : MonoBehaviour, IRouterTarget
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        [SerializeField] protected TankData _data;
        
        public TankHealth Health { get; protected set; }
        public TankView View { get; protected set; }
        public TankBattleData BattleData { get; protected set; }
        
        protected Input.Input _input;

        // TODO: Make base Initialize function
        // TODO: Use MVP/MVVM pattern
        
        private void FixedUpdate()
        {
            Vector2 moveInputVector = _input.GetMoveInput();
            _chassis.Move(moveInputVector);
            _chassis.Rotate(moveInputVector);
        }

        public void Die()
        {
            _input.Disable();
            View.OnDeath();
        }

        protected void Shoot()
        {
            if (_input.GetShootInput())
            {
                _gun.Shoot();
            }
        }
    }
}