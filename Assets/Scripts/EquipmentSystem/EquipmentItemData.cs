using System;
using UnityEngine;

namespace EquipmentSystem
{
    [Serializable]
    public class EquipmentItemData
    {
        [SerializeField] private EquipmentItemType _type;
        [SerializeField] private Sprite _sprite;
        
        public EquipmentItemType Type => _type;
        public Sprite Sprite => _sprite;
    }
}