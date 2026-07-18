using UnityEngine;

namespace InputSystem
{
    public interface IInput
    {
        public bool IsActive { get; }
        public void Enable();
        public void Disable();
        public Vector2 GetMoveInput();
        public bool GetShootInput();
    }
}