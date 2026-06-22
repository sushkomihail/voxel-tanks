using InputSystem;
using Tank.View;
using UnityEngine;
using VContainer;

namespace Tank
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : TankController
    {
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private Vector3 _cameraFollowingOffset = new(0f, 3f, -9.2f);
        
        public TankChassis Chassis => _chassis;
        public TankGun Gun => _gun;
        public TankView View => _view;
        
        private TankCamera _tankCamera;

        [Inject]
        public void Construct(TankCamera tankCamera)
        {
            _tankCamera = tankCamera;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            Input = GetComponent<PlayerInput>();
            Input.Initialize();
            Input.Enable();
            
            // TODO: Disable if its local player
            _view.DisableOverTankHealthBar();
        }

        private void Update()
        {
            Vector2 lookInputVector = ((PlayerInput)Input).GetLookInput();
            _tankCamera.Rotate(lookInputVector);
            _tankCamera.TryHighlightFocusObject();
            
            if (!Input.IsActive) return;
            
            Vector3 lookPosition = _tankCamera.CastRay();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
            
            Shoot();
        }

        private void LateUpdate()
        {
            _tankCamera.FollowTarget(_cameraTarget, _cameraFollowingOffset);
        }
    }
}