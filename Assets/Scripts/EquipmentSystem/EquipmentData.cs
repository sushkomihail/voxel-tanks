using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EquipmentSystem
{
    [CreateAssetMenu(menuName = "EquipmentSystem/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        [SerializeField] private List<EquipmentItemData> _items;

        public bool TryGetItemSprite(EquipmentItemType type, out Sprite sprite)
        {
            EquipmentItemData data = _items.FirstOrDefault(item => item.Type == type);

            if (data == null)
            {
                sprite = null;
                return false;
            }
            
            sprite = data.Sprite;
            return true;
        }
    }
}