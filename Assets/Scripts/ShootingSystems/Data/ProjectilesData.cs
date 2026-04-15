using System.Collections.Generic;
using UnityEngine;

namespace ShootingSystems.Data
{
    [CreateAssetMenu(fileName = "_Projectiles", menuName = "Projectiles Data")]
    public class ProjectilesData : ScriptableObject
    {
        [SerializeField] private ProjectileItem[] _items;
        
        public IReadOnlyList<ProjectileItem> Items => _items;
    }
}