using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public abstract class TankModuleData
    {
        [SerializeField] private float _maxHealth = 100f;
        
        public float MaxHealth => _maxHealth;
    }
}