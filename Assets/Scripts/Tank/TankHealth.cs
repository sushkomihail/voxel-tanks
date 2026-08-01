using System;
using ShootingSystems;
using Tank.Data;
using UnityEngine;
using UpgradeSystem;

namespace Tank
{
    public class TankHealth : IDamageable
    {
        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        private HealthData _data;
        private UpgradeManager _upgradeManager;
        private UpgradeBroker _upgradeBroker;
        private TankController _owner;
        private int _currentHealth;
        private int _maxHealth;

        public TankHealth(HealthData data, UpgradeManager upgradeManager, UpgradeBroker upgradeBroker, TankController owner)
        {
            _data = data;
            _upgradeManager = upgradeManager;
            _upgradeBroker = upgradeBroker;
            _owner = owner;
            
            _currentHealth = _data.MaxHealth;
            _maxHealth = _data.MaxHealth;
        }

        public void InvokeHealthChanged()
        {
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
        
        public void TakeDamage(int damage, object attacker)
        {
            if (_currentHealth == 0) return;
   
            ChangeHealth(_currentHealth - damage, _maxHealth);
            
            if (_currentHealth == 0)
            {
                OnDeath?.Invoke();
                _upgradeManager.UpgradeDamage(attacker);
                _upgradeManager.EnqueueUpgradePair();
            }
        }

        public void Heal(float percent)
        {
            int healAmount = (int)(_maxHealth * percent);
            ChangeHealth(_currentHealth + healAmount, _maxHealth);
        }

        public void QueryMaxHealth(object owner, StatType statType)
        {
            if (owner is not TankController controller) return;
            
            if (controller == _owner && statType == StatType.Health)
            {
                StatQuery query = new StatQuery(StatType.Health, _data.MaxHealth);
                _upgradeBroker.Query(_owner, query);
                
                float healthPercent = (float)_currentHealth / _maxHealth;
                _maxHealth = (int)query.Value;
                _currentHealth = (int)(_maxHealth * healthPercent);
                
                OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            }
        }

        private void ChangeHealth(int currentHealth, int maxHealth)
        {
            _currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }
    }
}