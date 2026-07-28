using System;
using System.Collections.Generic;
using System.Linq;
using InputSystem;
using Tank.Modules;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EquipmentSystem
{
    public class Equipment : IDisposable
    {
        public event Action<EquipmentItemType> OnItemCountChanged;
        public event Action<EquipmentItemType> OnItemSelected;
        public event Action<EquipmentItemType> OnItemDeselected;

        private readonly Dictionary<EquipmentItemType, EquipmentItemFactory> _itemFactories = new();
        private readonly Dictionary<EquipmentItemType, EquipmentItem> _items = new();
        private readonly GameInput _inputActions;
        private EquipmentItem _selectedItem;
        private List<Key> _bindings;

        public IReadOnlyList<EquipmentItemType> AvailableItemTypes { get; }
        public IReadOnlyList<Key> Bindings => _bindings;

        public Equipment(GameInput inputActions, IReadOnlyList<EquipmentItemType> availableItemTypes, TankModule[] tankModules)
        {
            _inputActions = inputActions;
            AvailableItemTypes = availableItemTypes;
            
            foreach (EquipmentItemType type in availableItemTypes)
            {
                _itemFactories.Add(type, type switch
                {
                    EquipmentItemType.RepairKit => new RepairKitFactory(tankModules),
                    _ => throw new ArgumentOutOfRangeException()
                });
            }

            foreach (EquipmentItemType type in availableItemTypes)
            {
                AddItems(type, 0);
            }

            InitializeBindings();
        }
        
        public void Dispose()
        {
            foreach (var (_, item) in _items)
            {
                item.OnUsed -= ResetSelectedItem;
                item.OnUsed -= EnableCollidingInputActions;
            }
        }

        public bool TryGetItem(EquipmentItemType type, out EquipmentItem item)
        {
            return _items.TryGetValue(type, out item);
        }

        public void AddItems(EquipmentItemType type, int count)
        {
            if (!_itemFactories.TryGetValue(type, out var factory)) return;

            if (_items.ContainsKey(type))
            {
                _items[type].UpdateCount(_items[type].Count + count);
            }
            else
            {
                EquipmentItem item = factory.CreateItem();
                item.UpdateCount(count);
                
                item.OnUsed += ResetSelectedItem;
                item.OnUsed += EnableCollidingInputActions;
                
                _items.Add(type, item);
            }
            
            OnItemCountChanged?.Invoke(type);
        }
        
        public void HandleItemSelection()
        {
            Keyboard keyboard = Keyboard.current;
            
            if (keyboard == null) return;
            
            if (keyboard[Key.Escape].wasPressedThisFrame && _selectedItem != null)
            {
                OnItemDeselected?.Invoke(_selectedItem.Type);
                EnableCollidingInputActions();
                ResetSelectedItem();
            }
            
            if (_selectedItem != null)
            {
                _selectedItem.TryUse();
                return;
            }

            for (int i = 0; i < _bindings.Count; i++)
            {
                if (keyboard[_bindings[i]].wasPressedThisFrame && i < _items.Count)
                {
                    var pair = _items.ElementAt(i);
                    _selectedItem = pair.Value;

                    if (_selectedItem.Count == 0)
                    {
                        ResetSelectedItem();
                        return;
                    }
                    
                    DisableCollidingInputActions();
                    OnItemSelected?.Invoke(pair.Key);
                    break;
                }
            }
        }
        
        private void InitializeBindings()
        {
            int bindingsCount = Mathf.Min(AvailableItemTypes.Count, 9);
            InputBinder.TryBindDigitKeys(bindingsCount, Key.Digit4, out _bindings);
        }
        
        private void ResetSelectedItem()
        {
            _selectedItem = null;
        }

        private void EnableCollidingInputActions()
        {
            _inputActions.Projectile.Select.Enable();
        }

        private void DisableCollidingInputActions()
        {
            _inputActions.Projectile.Select.Disable();
        }
    }
}