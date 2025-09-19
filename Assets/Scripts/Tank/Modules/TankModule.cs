using ShootingSystems;
using UnityEngine;

namespace Tank.Modules
{
    public abstract class TankModule : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health = 100f;

        private float _currentHealth;
        
        public bool IsDamaged { get; private set; }
        public bool IsCritical { get; private set; }

        public virtual void Init()
        {
            _currentHealth = _health;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth > 0 && !IsDamaged)
            {
                EnterDamagedState();
                IsDamaged = true;
            }
            else if (!IsCritical)
            {
                EnterCriticalState();
                IsCritical = true;
            }
        }
        
        public virtual void EnterDamagedState() {}
        
        public virtual void EnterCriticalState() {}
    }
}