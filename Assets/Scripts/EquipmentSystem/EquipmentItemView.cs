using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EquipmentSystem
{
    public class EquipmentItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private TMP_Text _bindingText;
        
        protected EquipmentItem _item;

        public virtual EquipmentItemType Type => EquipmentItemType.None;

        public virtual void Initialize(EquipmentItem item, Sprite sprite, int count, string binding)
        {
            _item = item;
            _icon.sprite = sprite;
            _countText.text = count.ToString();
            _bindingText.text = binding;

            _item.OnUsed += UpdateCountText;
            _item.OnUsed += OnDeselected;
        }

        private void OnDestroy()
        {
            _item.OnUsed -= UpdateCountText;
            _item.OnUsed -= OnDeselected;
        }

        public virtual void OnSelected() {}
        
        public virtual void OnDeselected() {}

        public void UpdateCountText()
        {
            _countText.text = _item.Count.ToString();
        }
    }
}