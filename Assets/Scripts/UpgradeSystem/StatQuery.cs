namespace UpgradeSystem
{
    public class StatQuery
    {
        public StatType StatType { get; }
        public float BaseValue { get; }
        public float Value { get; private set; }

        public StatQuery(StatType statType, float baseValue)
        {
            StatType = statType;
            BaseValue = baseValue;
            Value = baseValue;
        }

        public void SetValue(float value)
        {
            Value = value;
        }
    }
}