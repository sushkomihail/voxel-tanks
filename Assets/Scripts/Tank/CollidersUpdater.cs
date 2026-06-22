using System.Collections.Generic;
using UnityEngine;

namespace Tank
{
    public class CollidersUpdater : MonoBehaviour
    {
        private readonly Dictionary<Transform, Transform> _colliders = new();
        
        private void Update()
        {
            foreach ((Transform collider, Transform target) in _colliders)
            {
                target.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
                collider.position = position;
                collider.rotation = rotation;
            }
        }

        public void AddCollider(Transform collider, Transform target)
        {
            _colliders.Add(collider, target);
        }
    }
}