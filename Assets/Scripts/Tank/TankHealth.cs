using Managers;
using UnityEngine;

namespace Tank
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 1000;
        [SerializeField] private Armor.Armor[] _armorAreas;
        
        public int MaxHealth => _maxHealth;
        
        private TankHealthView _view;
        private int _currentHealth;
        private bool _isPlayer;

        public void Init(bool isPlayer)
        {
            _currentHealth = _maxHealth;
            _isPlayer = isPlayer;
            InitArmorAreas();
        }

        public void SetView(TankHealthView view)
        {
            _view = view;
        }

        public void OnArmorDamaged(int damage)
        {
            if (_currentHealth == 0) return;
            
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;

                if (!_isPlayer)
                {
                    KillsManager.Instance.IncreaseCounter();
                }
                // TODO: Destroy with effects
            }
            
            _view?.UpdateSlider(_currentHealth);
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