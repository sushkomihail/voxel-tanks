using System;
using UnityEngine;

namespace UpgradeSystem
{
    [CreateAssetMenu(fileName = "_Upgrade", menuName = "UpgradeSystem/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        [SerializeField, TextArea] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private StatType _statType;
        [SerializeField] private ModifierType _modifierType;
        [SerializeField] private float _modifierValue;
        
        public string Description => _description;
        public Sprite Icon => _icon;
        public StatType StatType => _statType;
        public float ModifierValue => _modifierValue;

        public StatModifier GetModifier(UpgradeBroker broker, object owner)
        {
            return _modifierType switch
            {
                ModifierType.Percentage => new PercentageModifier(broker, owner, _statType, _modifierValue),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}