using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(Rigidbody))]
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankChassis _chassis;
        [SerializeField] private TankTurret _turret;
        [SerializeField] private TankGun _gun;
        [SerializeField] private TankCamera _camera;

        private void Awake()
        {
            _chassis?.Init();
            _gun?.Init();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            PlayerInput.Instance.Controls.Tank.Shoot.performed += _ => _gun?.Shoot();
        }

        private void Update()
        {
            _turret.Rotate();
            _gun.Rotate();
            _camera.Rotate();
        }

        private void FixedUpdate()
        {
            _chassis.Move();
            _chassis.Rotate();
        }
    }
}