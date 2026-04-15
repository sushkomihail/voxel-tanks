using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] private int _maxHealth = 1000;
        
        public int MaxHealth => _maxHealth;
    }
}