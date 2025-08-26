using System;
using UnityEngine;

namespace ShootingSystems
{
    [Serializable]
    public class ProjectileItem
    {
        [SerializeField] private ProjectileType _type;
        [SerializeField] private Projectile _prefab;
        [SerializeField] private ProjectileProps _props;
        
        public ProjectileType Type => _type;
        public Projectile Prefab => _prefab;
        public ProjectileProps Props => _props;
    }
}