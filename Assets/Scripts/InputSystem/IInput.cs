using UnityEngine;

namespace InputSystem
{
    public interface IInput
    {
        public bool IsEnabled { get; }
        public void Enable();
        public void Disable();
        public Vector2 GetMoveInput();
        public Vector3 GetLookPoint();
        public bool GetShootInput();
        public void Destroy();
    }
}