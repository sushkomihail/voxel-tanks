using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UpgradesSelectorItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _iconImage;
        
        public event Action OnSelected;

        public void Initialize(string description, float value, Sprite icon)
        {
            _descriptionText.text = string.Format(description, value);
            _iconImage.sprite = icon;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnSelected?.Invoke();
        }
    }
}