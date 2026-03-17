using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody), typeof(TankHealth))]
    public abstract class Tank : MonoBehaviour
    {
        [SerializeField] protected TankChassis _chassis;
        [SerializeField] protected TankTurret _turret;
        [SerializeField] protected TankGun _gun;
        
        public TankHealth Health { get; private set; }
        
        protected IInput _input;
        
        protected virtual void Awake()
        {
            Health = GetComponent<TankHealth>();
            
            _chassis?.Init();
            _gun?.Init();
        }

        protected virtual void Start() {}

        protected virtual void Update()
        {
            if (_input.GetShootInput())
            {
                _gun.Shoot();
            }
        }

        protected virtual void FixedUpdate()
        {
            Vector2 moveInputVector = _input.GetMoveInput();
            _chassis.Move(moveInputVector);
            _chassis.Rotate(moveInputVector);
        }
    }
}