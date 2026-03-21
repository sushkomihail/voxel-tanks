using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankHealth), typeof(TankView))]
    public abstract class Tank : MonoBehaviour
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        
        public TankHealth Health { get; protected set; }
        public TankView View { get; protected set; }
        
        protected Input.Input _input;

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