using UnityEngine;

namespace Input
{
    public abstract class Input : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }
        
        public abstract Vector2 GetMoveInput();
        
        public abstract bool GetShootInput();
    }
}