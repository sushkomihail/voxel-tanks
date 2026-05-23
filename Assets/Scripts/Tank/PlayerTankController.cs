using InputSystem;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerTankController : TankController
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private Vector3 _cameraFollowingOffset = new(0f, 3f, -9.2f);
        
        public TankGun Gun => _gun;
        public TankView View => _view;

        public Transform Center => _center;
        
        private TankCamera _camera;

        public void Initialize(TankCamera camera)
        {
            Initialize();
            
            _camera = camera;
            
            Input = GetComponent<PlayerInput>();
            Input.Enable();
        }

        private void Update()
        {
            Vector2 lookInputVector = ((PlayerInput)Input).GetLookInput();
            _camera.Rotate(lookInputVector);
            _camera.TryHighlightFocusObject();
            
            if (!Input.IsActive) return;
            
            Vector3 lookPosition = _camera.CastRay();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            Shoot();
        }

        private void LateUpdate()
        {
            _camera.FollowTarget(_cameraTarget, _cameraFollowingOffset);
        }
    }
}