using System;
using Managers;
using UnityEngine;

namespace Tank
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 1000;
        [SerializeField] private Armor.Armor[] _armorAreas;
        
        public event Action<int, int> OnHealthChanged;

        private Tank _tank;
        private int _currentHealth;
        private bool _isPlayer;

        public void Init(Tank tank)
        {
            _currentHealth = _maxHealth;
            _tank = tank;
            _isPlayer = tank is PlayerTank;
            InitArmorAreas();
        }

        public void OnArmorDamaged(int damage)
        {
            if (_currentHealth == 0) return;
            
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            
            if (_currentHealth == 0)
            {
                if (!_isPlayer)
                {
                    KillsManager.Instance.IncreaseCounter();
                }
                
                _tank.Die();
            }
        }

        private void InitArmorAreas()
        {
            foreach (Armor.Armor armorArea in _armorAreas)
            {
                armorArea.Init(this);
            }
        }
    }
}