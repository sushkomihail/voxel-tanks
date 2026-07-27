using UnityEngine;

namespace EquipmentSystem
{
    public class RepairKitView : EquipmentItemView
    {
        [SerializeField] private TargetsMenu _repairMenu;
        [SerializeField] private RepairMenuData _repairMenuData;
        
        public override EquipmentItemType Type => EquipmentItemType.RepairKit;

        public override void Initialize(EquipmentItem item, Sprite sprite, int count, string binding)
        {
            base.Initialize(item, sprite, count, binding);
            
            _repairMenu.Initialize(GetMenuSprites(), GetMenuBindings());
            _repairMenu.gameObject.SetActive(false);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            _repairMenu.gameObject.SetActive(true);
        }

        public override void OnDeselected()
        {
            base.OnDeselected();
            _repairMenu.gameObject.SetActive(false);
        }

        private Sprite[] GetMenuSprites()
        {
            RepairKit repairKit = (RepairKit)_item;
            var tankModules = repairKit.TankModules;
            
            var sprites = new Sprite[tankModules.Count];
            
            for (int i = 0; i <  tankModules.Count; i++)
            {
                if (!_repairMenuData.TryGetItemSprite(tankModules[i].Type, out Sprite sprite)) continue;
                
                sprites[i] = sprite;
            }
            
            return sprites;
        }

        private string[] GetMenuBindings()
        {
            RepairKit repairKit = (RepairKit)_item;

            var bindings = new string[repairKit.TankModules.Count];
            
            for (int i = 0; i < repairKit.Bindings.Count; i++)
            {
                string binding = repairKit.Bindings[i].ToString();

                if (binding.StartsWith("Digit"))
                {
                    bindings[i] = binding[5..];
                }
                else
                {
                    bindings[i] = binding;
                }
            }
            
            return bindings;
        }
    }
}