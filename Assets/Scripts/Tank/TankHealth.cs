using UnityEngine;

namespace Tank
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private int _health = 1000;
        [SerializeField] private Armor.Armor[] _armorAreas;
        
        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _health;
            InitArmorAreas();
        }

        public void OnArmorDamaged(int damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                // TODO: Defeat
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