using System.Collections.Generic;
using Projectiles;
using UnityEngine;

namespace ShootingSystems.Data
{
    [CreateAssetMenu(fileName = "_Projectiles", menuName = "Projectiles Data")]
    public class ProjectilesData : ScriptableObject
    {
        [SerializeField] private ProjectileItem[] _items;
        
        public IReadOnlyList<ProjectileItem> Items => _items;
        
        private readonly Dictionary<ProjectileType, ProjectileProps> _props = new();

        public void ExtractProps()
        {
            foreach (ProjectileItem item in _items)
            {
                _props.Add(item.Type, item.Props);
            }
        }

        public bool TryGetPropsByType(ProjectileType type, out ProjectileProps props)
        {
            return _props.TryGetValue(type, out props);
        }
    }
}