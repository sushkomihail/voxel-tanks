using UnityEngine;

namespace InputSystem
{
    public abstract class Input : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        
        public abstract void Initialize();

        public virtual void Enable()
        {
            IsActive = true;
        }

        public virtual void Disable()
        {
            IsActive = false;
        }
        
        public abstract Vector2 GetMoveInput();
        
        public abstract bool GetShootInput();
    }
}