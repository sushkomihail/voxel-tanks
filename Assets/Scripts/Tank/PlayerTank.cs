using Input;
using Tank.Data;
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

        public void Initialize(TankCamera camera)
        {
            Health = GetComponent<TankHealth>();
            Health.Initialize(_data.HealthData);
            
            _camera = camera;
            
            View = GetComponent<TankView>();
            View.Initialize();
            
            _input = GetComponent<PlayerInput>();
            _input.Enable();
            
            BattleData = new TankBattleData();
            
            _chassis.Initialize(_data.ChassisData, _data.EngineData, _data.TrackData);
            _turret.Initialize(_data.TurretData);
            _gun.Initialize(_data.GunData);
        }

        private void Update()
        {
            Vector2 lookInputVector = ((PlayerInput)_input).GetLookInput();
            _camera.Rotate(lookInputVector);
            _camera.TryHighlightFocusObject();
            
            if (!_input.IsActive) return;
            
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