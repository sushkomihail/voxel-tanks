using Tank;
using UnityEngine;

namespace InputSystem
{
    public class PlayerInput : IInput
    {
        private readonly TankCamera _tankCamera;
        
        public GameInput Actions { get; }
        public bool IsEnabled { get; private set; }

        public PlayerInput(GameInput actions, TankCamera tankCamera)
        {
            Actions = actions;
            Actions?.Enable();
            
            _tankCamera = tankCamera;
            
            Enable();
        }

        public void Destroy()
        {
            Actions?.Dispose();
        }
        
        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public Vector2 GetMoveInput()
        {
            if (!IsEnabled || Actions == null) return Vector2.zero;
            
            return Actions.Tank.Move.ReadValue<Vector2>();
        }

        public Vector3 GetLookPoint()
        {
            if (!_tankCamera) return Vector3.zero;
            
            Vector2 lookInputVector;

            if (!IsEnabled || Actions == null)
            {
                lookInputVector = Vector2.zero;
            }
            else
            {
                lookInputVector = Actions.Tank.Look.ReadValue<Vector2>();
            }
            
            _tankCamera.Rotate(lookInputVector);
            return _tankCamera.CastRay();
        }

        public bool GetShootInput()
        {
            if (!IsEnabled || Actions == null) return false;
            
            return Actions.Tank.Shoot.IsPressed();
        }
    }
}