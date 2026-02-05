using System;
using UnityEngine;

namespace ShootingSystems
{
    [Serializable]
    public class ProjectileProps
    {
        [SerializeField] private float _speed = 500;
        [SerializeField] private int _baseDamage = 300;
        [SerializeField] private int _damageByModules = 50;

        public float Speed => _speed;
        public int BaseDamage => _baseDamage;
        public int DamageByModules => _damageByModules;
    }
}