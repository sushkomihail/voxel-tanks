using System;

namespace UpgradeSystem
{
    public abstract class StatModifier : IDisposable
    {
        protected readonly object _owner;
        protected readonly StatType _statType;
        
        private readonly UpgradeBroker _broker;

        protected StatModifier(UpgradeBroker broker, object owner, StatType statType)
        {
            _broker = broker;
            _owner = owner;
            _statType = statType;
            
            _broker.OnStatQuery += HandleQuery;
        }

        public void Dispose()
        {
            _broker.OnStatQuery -= HandleQuery;
            _broker.NotifyModifierChanged(_owner, _statType);
        }

        protected abstract void HandleQuery(object sender, StatQuery query);
    }
}