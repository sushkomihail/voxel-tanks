using System;
using System.Collections.Generic;
using InputSystem;
using JetBrains.Annotations;
using ShootingSystems;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = InputSystem.PlayerInput;

namespace UI
{
    public class ProjectilesSelector : MonoBehaviour
    {
        [SerializeField] private ProjectilesSelectorData _data;
        [SerializeField] private ProjectilesSelectorItem _itemDataPrefab;
        
        public event Action OnCurrentItemChanged;
        public event Action<ProjectileType> OnNextItemChanged;
        
        private readonly Dictionary<string, int> _keyCodes = new();
        private readonly List<ProjectilesSelectorItem> _items = new();
        [CanBeNull] private ProjectilesSelectorItem _currentItem;
        [CanBeNull] private ProjectilesSelectorItem _nextItem;

        public void Initialize(IReadOnlyList<ProjectileType> projectileTypes, PlayerInput input)
        {
            input.Actions.Projectile.Select.performed += HandleSelection;
            
            foreach (ProjectileType type in projectileTypes)
            {
                if (_data.TryGetItemDataByProjectileType(type, out ProjectilesSelectorItemData data))
                {
                    ProjectilesSelectorItem item = Instantiate(_itemDataPrefab, transform);
                    item.Initialize(data);
                    item.OnSelected += HandleSelection;
                    _items.Add(item);
                }
            }

            if (_items.Count != 0)
            {
                _items[0].SelectAsCurrent();
                _currentItem = _items[0];
            }
            
            AddSelectorBindings(projectileTypes.Count, input);
        }

        private void OnDestroy()
        {
            foreach (ProjectilesSelectorItem item in _items)
            {
                item.OnSelected -= HandleSelection;
            }
        }
        
        public void SetNextItemAsCurrent()
        {
            _currentItem?.Deselect();
            _nextItem?.SelectAsCurrent();
            _currentItem = _nextItem;
            _nextItem = null;
        }
        
        private void AddSelectorBindings(int bindingsNumber, PlayerInput input)
        {
            if (bindingsNumber == 0) return;

            for (int i = 1; i <= bindingsNumber; i++)
            {
                _keyCodes.Add(i.ToString(), i - 1);
                input.Actions.Projectile.Select.AddBinding($"<keyboard>/{i}");
            }
        }
        
        private void HandleSelection(InputAction.CallbackContext context)
        {
            string key = context.control.name;
            ProjectilesSelectorItem item = _items[_keyCodes[key]];
            HandleSelection(item);
        }

        private void HandleSelection(ProjectilesSelectorItem item)
        {
            if (item == _currentItem)
            {
                _nextItem?.Deselect();
                _nextItem = null;
                return;
            }

            if (item == _nextItem)
            {
                SetNextItemAsCurrent();
                OnCurrentItemChanged?.Invoke();
                return;
            }
            
            _nextItem?.Deselect();
            item.SelectAsNext();
            _nextItem = item;
            OnNextItemChanged?.Invoke(_nextItem.Data.Type);
        }
    }
}
