using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public class Tracer : MonoBehaviour
    {
        [SerializeField] private Transform _head;
        [SerializeField] private TrailRenderer[] _trailRenderers;
        [SerializeField] private TracerColorComparator _colorComparator;

        public void Initialize(ProjectileType projectileType, float scale)
        {
            _head.localScale = Vector3.one * scale;
            
            bool isColorFound = _colorComparator.TryGetColorByProjectileType(projectileType, out var color);
            
            foreach (TrailRenderer trailRenderer in _trailRenderers)
            {
                trailRenderer.startWidth = scale;
                trailRenderer.endWidth = scale;

                if (isColorFound)
                {
                    trailRenderer.startColor = color;
                    trailRenderer.endColor = color;
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            foreach (TrailRenderer trailRenderer in _trailRenderers)
            {
                trailRenderer.enabled = enabled;
            }
        }

        public void UpdateLocation(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}