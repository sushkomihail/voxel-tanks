using System.Collections.Generic;
using Databases;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Menu
{
    public class TankSelectionPanel : MonoBehaviour
    {
        [SerializeField] private TanksDatabase _tanksDatabase;
        [SerializeField] private Transform _itemsHolder;
        [SerializeField] private TankSelectionItem _itemPrefab;
        [SerializeField] private TankPreview _tankPreview;
        
        private readonly List<TankSelectionItem> _items = new();
        private TankSelectionItem _selectedItem;

        private void Awake()
        {
            CreateItems();
        }

        private void OnEnable()
        {
            TankSelectionItem.OnSelected += OnItemSelected;
        }

        private void OnDisable()
        {
            TankSelectionItem.OnSelected -= OnItemSelected;
        }

        public int GetSelectedTankId()
        {
            if (!_selectedItem) return -1;
            
            return _tanksDatabase.GetId(_selectedItem.TankData);
        }

        private void CreateItems()
        {
            foreach (var tank in _tanksDatabase.Tanks)
            {
                var item = Instantiate(_itemPrefab, _itemsHolder);
                item.Init(tank);
                _items.Add(item);
            }
        }

        private void OnItemSelected(TankSelectionItem item)
        {
            _selectedItem?.SetOutlineEnabled(false);
            _selectedItem?.SetSelected(false);
            _selectedItem = item;
            
            _tankPreview.PlaceTank(item.GetPreviewTankPrefab());
        }
    }
}