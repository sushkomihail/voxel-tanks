using System;
using UnityEngine;

namespace Projectiles
{
    [Serializable]
    public class ProjectileProps
    {
        [SerializeField] private float _scale = 0.2f;
        [SerializeField] private int _caliber = 100;
        [SerializeField] private float _speed = 500f;
        [SerializeField] private float _maxFlightDistance = 50f;
        [SerializeField] private int _penetration = 100;
        [SerializeField] private float _splashRadius;
        [SerializeField] private int _armorDamage = 300;
        [SerializeField] private int _moduleDamage = 50;
        [SerializeField] private LayerMask _hitMask = (1 << 3) | (1 << 9) | (1 << 10) | (1 << 11);
        [SerializeField] private LayerMask _armorMask = 1 << 11;
        
        public float Scale => _scale;
        public int Caliber => _caliber;
        public float Speed => _speed;
        public float MaxFlightDistance => _maxFlightDistance;
        public virtual int Penetration => _penetration;
        public float SplashRadius => _splashRadius;
        public virtual int ArmorDamage => _armorDamage;
        public int ModuleDamage => _moduleDamage;
        public LayerMask HitMask => _hitMask;
        public LayerMask ArmorMask => _armorMask;
    }
}