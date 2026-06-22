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
        
        public Dictionary<ProjectileType, ProjectileProps> ExtractProps()
        {
            Dictionary<ProjectileType, ProjectileProps> props = new();
            
            foreach (ProjectileItem item in _items)
            {
                props.Add(item.Type, item.Props);
            }
            
            return props;
        }
    }
}