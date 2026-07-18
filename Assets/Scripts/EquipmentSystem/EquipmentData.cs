using System;
using System.Collections.Generic;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public class EquipmentData
    {
        [SerializeField] private EquipmentItemType[] _availableTypes;
        
        public IReadOnlyList<EquipmentItemType> AvailableTypes => _availableTypes;
    }
}