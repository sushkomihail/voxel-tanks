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
        
        [Space(5), Header("Camera")]
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private Vector3 _cameraFollowingOffset = new(0f, 3f, -9.2f);
        
        // TODO: Set camera on spawn
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
            Vector3 lookPosition = _camera.CastRay();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            _camera.Rotate();
            
        }

        private void FixedUpdate()
        {
            Vector2 moveInputVector = PlayerInput.Instance.GetMoveInputVector();
            _chassis.Move(moveInputVector);
            _chassis.Rotate(moveInputVector);
        }

        private void LateUpdate()
        {
            _camera.FollowTarget(_cameraTarget, _cameraFollowingOffset);
        }
    }
}