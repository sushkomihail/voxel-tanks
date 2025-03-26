using System;
using UnityEngine;

namespace ShootingSystems
{
    [Serializable]
    public class ProjectileData
    {
        [SerializeField] private float _flightSpeed;
        [SerializeField] private int _damage;

        public float FlightSpeed => _flightSpeed;
        public int Damage => _damage;
    }
}