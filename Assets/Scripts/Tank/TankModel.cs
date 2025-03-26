using Input;
using UnityEngine;

namespace Tank
{
    public class TankModel : MonoBehaviour
    {
        [SerializeField] private TankChassis _chassis;
        [SerializeField] private TankTurret _turret;
        [SerializeField] private TankGun _gun;

        private TankInput _input;

        public void Initialize(TankInput input)
        {
            _input = input;
            _chassis.Initialize();
            _gun.Initialize(_input);
        }

        public void OnUpdate()
        {
            _chassis.ReadInput(_input);
            _chassis.Rotate();
            _turret.Rotate();
            _gun.Rotate();
        }

        public void OnFixedUpdate()
        {
            _chassis.Move();
        }
    }
}