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

        public void Init()
        {
            _currentHealth = _health;
            EnterNormalState();
        }

        public void TakeDamage(ProjectileProps props)
        {
            _currentHealth -= props.ModuleDamage;

            if (_currentHealth > 0 && !IsDamaged)
            {
                EnterDamagedState();
                IsDamaged = true;
            }
            
            if (_currentHealth <= 0 && !IsCritical)
            {
                EnterCriticalState();
                IsCritical = true;
            }
        }

        public virtual void EnterNormalState() {}

        protected virtual void EnterDamagedState() {}

        protected virtual void EnterCriticalState() {}
    }
}