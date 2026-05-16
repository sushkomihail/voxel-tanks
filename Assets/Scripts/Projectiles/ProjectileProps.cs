using System;
using UnityEngine;

namespace Projectiles
{
    [Serializable]
    public class ProjectileProps
    {
        [SerializeField] private float _speed = 500f;
        [SerializeField] private int _penetration = 100;
        [SerializeField] private float _splashRadius;
        [SerializeField] private int _armorDamage = 300;
        [SerializeField] private int _moduleDamage = 50;
        
        public float Speed => _speed;
        public int Penetration => _penetration;
        public float SplashRadius => _splashRadius;
        public virtual int ArmorDamage => _armorDamage;
        public int ModuleDamage => _moduleDamage;
    }
}