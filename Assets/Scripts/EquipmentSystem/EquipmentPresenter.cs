using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EquipmentSystem
{
    public class EquipmentPresenter : MonoBehaviour
    {
        [SerializeField] private EquipmentData _data;
        [SerializeField] private EquipmentItemView _baseItemViewPrefab;
        [SerializeField] private EquipmentItemView[] _specialItemViewPrefabs;
        [SerializeField] private Transform _container;
        
        private readonly Dictionary<EquipmentItemType, EquipmentItemView> _itemViews = new();
        private Equipment _equipment;

        public void Initialize(Equipment equipment)
        {
            _equipment = equipment;
            
            InstantiateItemViews();
            
            _equipment.OnItemCountChanged += OnItemCountChanged;
            _equipment.OnItemSelected += OnItemSelected;
            _equipment.OnItemDeselected += OnItemDeselected;
        }

        private void OnDestroy()
        {
            _equipment.OnItemCountChanged -= OnItemCountChanged;
            _equipment.OnItemSelected -= OnItemSelected;
            _equipment.OnItemDeselected -= OnItemDeselected;
        }

        private void Update()
        {
            _equipment.HandleItemSelection();
        }

        private void InstantiateItemViews()
        {
            for (int i = 0; i < _equipment.AvailableItemTypes.Count; i++)
            {
                EquipmentItemType type = _equipment.AvailableItemTypes[i];
                
                if (!_data.TryGetItemSprite(type, out Sprite sprite)) continue;

                if (!_equipment.TryGetItem(type, out EquipmentItem item)) continue;
                
                EquipmentItemView view = Instantiate(GetItemViewPrefab(type), _container);

                if (i < _equipment.Bindings.Count)
                {
                    view.Initialize(item, sprite, 0, _equipment.Bindings[i].ToString()[5..]);
                }
                else
                {
                    view.Initialize(item, sprite, 0, "");
                }
                
                _itemViews.Add(type, view);
            }
        }

        private EquipmentItemView GetItemViewPrefab(EquipmentItemType type)
        {
            EquipmentItemView prefab = 
                _specialItemViewPrefabs.FirstOrDefault(view => view.Type == type);
            
            if (!prefab) return _baseItemViewPrefab;
            return prefab;
        }

        private void OnItemCountChanged(EquipmentItemType type)
        {
            if (_itemViews.TryGetValue(type, out EquipmentItemView view))
            {
                view.UpdateCountText();
            }
        }

        private void OnItemSelected(EquipmentItemType type)
        {
            if (_itemViews.TryGetValue(type, out EquipmentItemView view))
            {
                view.OnSelected();
            }
        }

        private void OnItemDeselected(EquipmentItemType type)
        {
            if (_itemViews.TryGetValue(type, out EquipmentItemView view))
            {
                view.OnDeselected();
            }
        }
    }
}