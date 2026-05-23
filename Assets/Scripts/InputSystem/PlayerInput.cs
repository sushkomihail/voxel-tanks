using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class PlayerInput : Input
    {
        public PlayerControls Controls { get; private set; }

        private void Awake()
        {
            Controls = new PlayerControls();
        }
        
        private void OnEnable()
        {
            Controls.Enable();
        }

        private void OnDisable()
        {
            Controls.Disable();
        }

        public override Vector2 GetMoveInput()
        {
            if (!IsActive) return Vector2.zero;
            
            return Controls.Tank.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            return Controls.Tank.Look.ReadValue<Vector2>();
        }

        public override bool GetShootInput()
        {
            if (!IsActive) return false;
            
            return Controls.Tank.Shoot.IsPressed();
        }
    }
}