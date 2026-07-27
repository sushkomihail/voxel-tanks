using System;
using ShootingSystems;
using Tank.Data;
using UnityEngine;

namespace Tank.Modules
{
    public abstract class TankModule : MonoBehaviour, IDamageable
    {
        public event Action OnNormalStateEntered;
        public event Action OnDamagedStateEntered;
        public event Action OnCriticalStateEntered;
        
        protected TankModuleData _data;
        
        private float _currentHealth;
        private bool _isDamaged;
        private bool _isCritical;
        
        public abstract TankModuleType Type { get; }

        public void Initialize(TankModuleData data)
        {
            _data = data;
            _currentHealth = _data.MaxHealth;
            EnterNormalState();
        }

        public void TakeDamage(int damage, object attacker)
        {
            _currentHealth = Mathf.Max(0f, _currentHealth - damage);

            if (_currentHealth > 0f && !_isDamaged)
            {
                EnterDamagedState();
                _isDamaged = true;
            }
            
            if (_currentHealth == 0f && !_isCritical)
            {
                EnterCriticalState();
                _isCritical = true;
            }
        }

        public virtual void EnterNormalState()
        {
            OnNormalStateEntered?.Invoke();
        }

        protected virtual void EnterDamagedState()
        {
            OnDamagedStateEntered?.Invoke();
        }

        protected virtual void EnterCriticalState()
        {
            OnCriticalStateEntered?.Invoke();
        }
    }
}