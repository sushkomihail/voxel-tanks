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

        public void Init()
        {
            _currentHealth = _maxHealth;
            InitArmorAreas();
        }

        public void SetView(TankHealthView view)
        {
            _view = view;
        }

        public void OnArmorDamaged(int damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                // TODO: Destroy with effects
            }
            
            _view.UpdateSlider(_currentHealth);
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