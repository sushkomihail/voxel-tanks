using UnityEngine;

namespace InputSystem
{
    public class PlayerInput : Input
    {
        public PlayerControls Controls { get; private set; }

        public override void Initialize()
        {
            Controls = new PlayerControls();
            Controls.Enable();
        }

        private void OnDestroy()
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