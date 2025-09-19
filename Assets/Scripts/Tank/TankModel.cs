using UnityEngine;

namespace Tank
{
    public class TankModel : MonoBehaviour
    {
        [SerializeField] private TankChassis _chassis;
        [SerializeField] private TankTurret _turret;
        [SerializeField] private TankGun _gun;

        public void Init()
        {
            _chassis.Init();
            _gun.Init();
        }

        public void OnUpdate()
        {
            // _chassis.ReadInput(_input);
            // _chassis.Rotate();
            _turret.Rotate();
            _gun.Rotate();
        }

        public void OnFixedUpdate()
        {
            _chassis.Move();
            _chassis.Rotate();
        }
    }
}