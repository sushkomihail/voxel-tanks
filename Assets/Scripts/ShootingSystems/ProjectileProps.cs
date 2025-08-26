using System;
using UnityEngine;

namespace ShootingSystems
{
    [Serializable]
    public class ProjectileProps
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _baseDamage;

        public float Speed => _speed;
        public int BaseDamage => _baseDamage;
    }
}