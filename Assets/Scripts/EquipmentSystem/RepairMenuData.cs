using System;
using Tank.Modules;
using UnityEngine;

namespace EquipmentSystem
{
    [CreateAssetMenu(fileName = "RepairMenuData", menuName = "EquipmentSystem/RepairMenuData")]
    public class RepairMenuData : ScriptableObject
    {
        [Serializable]
        private struct RepairMenuItemData
        {
            [SerializeField] private TankModuleType _type;
            [SerializeField] private Sprite _sprite;
            
            public TankModuleType Type => _type;
            public Sprite Sprite => _sprite;
        }
        
        [SerializeField] private RepairMenuItemData[] _repairMenuItems;

        public bool TryGetItemSprite(TankModuleType type, out Sprite sprite)
        {
            foreach (RepairMenuItemData item in _repairMenuItems)
            {
                if (item.Type == type)
                {
                    sprite = item.Sprite;
                    return true;
                }
            }
            
            sprite = null;
            return false;
        }
    }
}