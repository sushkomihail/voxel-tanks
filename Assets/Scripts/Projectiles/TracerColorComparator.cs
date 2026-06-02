using System;
using System.Collections.Generic;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    [CreateAssetMenu(fileName = "TracerColorComparator", menuName = "Projectiles/TracerColorComparator")]
    public class TracerColorComparator : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct Comparison
        {
            public ProjectileType ProjectileType;
            public Color Color;
        }
        
        [SerializeField] private Comparison[] _comparisons;
        
        private readonly Dictionary<ProjectileType, Color> _colorMap = new();
        
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _colorMap.Clear();
            
            foreach (var comparison in _comparisons)
            {
                _colorMap.TryAdd(comparison.ProjectileType, comparison.Color);
            }
        }

        public bool TryGetColorByProjectileType(ProjectileType projectileType, out Color color)
        {
            return _colorMap.TryGetValue(projectileType, out color);
        }
    }
}