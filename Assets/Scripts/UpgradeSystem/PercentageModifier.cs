namespace UpgradeSystem
{
    public class PercentageModifier : StatModifier
    {
        private readonly StatType _targetStat;
        private readonly float _multiplier;

        public PercentageModifier(UpgradeBroker broker, object owner, StatType statType, float percent) 
            : base(broker, owner, statType)
        {
            _targetStat = _statType;
            _multiplier = 1f + percent / 100;
            
            broker.NotifyModifierChanged(_owner, _statType);
        }

        protected override void HandleQuery(object sender, StatQuery query)
        {
            if (sender == _owner && query.StatType == _targetStat)
            {
                query.SetValue(query.Value * _multiplier);
            }
        }
    }
}