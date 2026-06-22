using System;
using Tank.Data;
using UnityEngine;

namespace Tank
{
    public class TankHealth
    {
        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        private HealthData _data;
        private int _currentHealth;

        public void Initialize(HealthData data)
        {
            _data = data;
            _currentHealth = _data.MaxHealth;
            
            OnHealthChanged?.Invoke(_currentHealth, _data.MaxHealth);
        }

        public void OnArmorDamaged(int damage)
        {
            if (_currentHealth == 0) return;
            
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke(_currentHealth, _data.MaxHealth);
            
            if (_currentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}