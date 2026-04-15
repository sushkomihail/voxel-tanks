using System;
using UnityEngine;

namespace ShootingSystems
{
    [Serializable]
    public class ProjectileProps
    {
        [SerializeField] private float _speed = 500f;
        [SerializeField] private int _penetration = 100;
        [SerializeField] private int _armorDamage = 300;
        [SerializeField] private int _moduleDamage = 50;
        
        public float Speed => _speed;
        public int Penetration => _penetration;
        public int ArmorDamage => _armorDamage;
        public int ModuleDamage => _moduleDamage;
    }
}