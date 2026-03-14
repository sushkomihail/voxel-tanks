using UnityEngine;

namespace Input
{
    public interface IInput
    {
        public Vector2 GetMoveInput();
        public bool GetShootInput();
    }
}