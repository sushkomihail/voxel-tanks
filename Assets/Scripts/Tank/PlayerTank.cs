using Input;
using UnityEngine;

namespace Tank
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerTank : Tank
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private Vector3 _cameraFollowingOffset = new(0f, 3f, -9.2f);

        public TankGun Gun => _gun;
        public Transform Center => _center;
        
        private TankCamera _camera;

        protected override void Awake()
        {
            base.Awake();
            _input = GetComponent<PlayerInput>();
        }

        protected override void Update()
        {
            base.Update();
            
            Vector2 lookInputVector = ((PlayerInput)_input).GetLookInput();
            _camera.Rotate(lookInputVector);
            
            Vector3 lookPosition = _camera.CastRay();
            _turret.Rotate(lookPosition);
            _gun.Rotate(lookPosition);
        }

        private void LateUpdate()
        {
            _camera.FollowTarget(_cameraTarget, _cameraFollowingOffset);
        }

        public void SetCamera(TankCamera camera)
        {
            _camera = camera;
        }
    }
}