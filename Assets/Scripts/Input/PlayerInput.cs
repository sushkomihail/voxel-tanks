using UnityEngine;

namespace Input
{
    public class PlayerInput : Input
    {
        private PlayerControls _controls;

        private void Awake()
        {
            _controls = new PlayerControls();
        }
        
        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public override Vector2 GetMoveInput()
        {
            if (!IsActive) return Vector2.zero;
            
            return _controls.Tank.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            return _controls.Tank.Look.ReadValue<Vector2>();
        }

        public override bool GetShootInput()
        {
            if (!IsActive) return false;
            
            return _controls.Tank.Shoot.IsPressed();
        }
    }
}