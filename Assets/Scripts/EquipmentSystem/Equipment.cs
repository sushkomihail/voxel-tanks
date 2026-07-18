using System;
using System.Collections.Generic;

namespace EquipmentSystem
{
    public class Equipment
    {
        private class Item
        {
            public int Count { get; set; }
            public IEquipmentItem Instance { get; set; }
        }

        private readonly Dictionary<EquipmentItemType, EquipmentItemFactory> _itemFactories = new();
        private readonly Dictionary<EquipmentItemType, Item> _items = new();

        public Equipment(EquipmentData data)
        {
            foreach (EquipmentItemType type in data.AvailableTypes)
            {
                _itemFactories.Add(type, type switch
                {
                    EquipmentItemType.RepairKit => new RepairKitFactory(),
                    _ => throw new ArgumentOutOfRangeException()
                });
            }
        }

        public void AddItems(EquipmentItemType type, int count)
        {
            if (!_itemFactories.TryGetValue(type, out var factory)) return;
            
            if (!_items.TryAdd(type, new Item { Count = count, Instance = factory.CreateItem() }))
            {
                _items[type].Count += count;
            }
        }
    }
}