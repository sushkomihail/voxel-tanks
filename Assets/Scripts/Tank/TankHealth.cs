using System;
using Tank.Data;
using UnityEngine;

namespace Tank
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private ArmorSystem.Armor[] _armorAreas;
        
        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        private HealthData _data;
        private int _currentHealth;

        public void Initialize(HealthData data)
        {
            _data = data;
            _currentHealth = _data.MaxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _data.MaxHealth);
            
            InitArmorAreas();
        }

        public void OnArmorDamaged(int damage)
        {
            if (_currentHealth == 0) return;
            
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _data.MaxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _data.MaxHealth);
            
            if (_currentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }

        private void InitArmorAreas()
        {
            foreach (ArmorSystem.Armor armorArea in _armorAreas)
            {
                armorArea.Initialize(this);
            }
        }
    }
}