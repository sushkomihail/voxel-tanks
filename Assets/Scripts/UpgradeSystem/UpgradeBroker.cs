using System;

namespace UpgradeSystem
{
    public class UpgradeBroker
    {
        public event Action<object, StatQuery> OnStatQuery;
        public event Action<object, StatType> OnStatModifierChanged;

        public void Query(object sender, StatQuery query)
        {
            OnStatQuery?.Invoke(sender, query);
        }
        
        public void NotifyModifierChanged(object owner, StatType statType)
        {
            OnStatModifierChanged?.Invoke(owner, statType);
        }
    }
}