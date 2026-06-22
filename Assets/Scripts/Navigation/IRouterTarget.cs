using UnityEngine;

namespace Navigation
{
    public interface IRouterTarget
    {
        public Vector3 Position { get; }
        public bool IsActive { get; }
    }
}