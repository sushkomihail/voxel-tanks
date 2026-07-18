using UnityEngine;

namespace InputSystem
{
    public class PlayerInput : IInput
    {
        public GameInput Actions { get; }
        public bool IsActive { get; private set; }

        public PlayerInput(GameInput actions)
        {
            Actions = actions;
            Actions.Enable();
        }
        
        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }

        public Vector2 GetMoveInput()
        {
            if (!IsActive) return Vector2.zero;
            
            return Actions.Tank.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            if (!IsActive) return Vector2.zero;
            
            return Actions.Tank.Look.ReadValue<Vector2>();
        }

        public bool GetShootInput()
        {
            if (!IsActive) return false;
            
            return Actions.Tank.Shoot.IsPressed();
        }
    }
}