using Projectiles;
using ShootingSystems;
using Tank.Data;
using UnityEngine;

namespace Tank.Modules
{
    public abstract class TankModule<T> : MonoBehaviour, IDamageable where T : TankModuleData
    {
        public bool IsDamaged { get; private set; }
        public bool IsCritical { get; private set; }
        
        protected T _data;
        
        private float _currentHealth;

        public void Initialize(T data)
        {
            _data = data;
            _currentHealth = _data.MaxHealth;
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