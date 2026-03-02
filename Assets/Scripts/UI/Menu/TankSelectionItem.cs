using System;
using Databases;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu
{
    [RequireComponent(typeof(Outline))]
    public class TankSelectionItem : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _tankImage;
        [SerializeField] private TMP_Text _tankName;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private Color _activeColor;
        
        public static event Action<TankSelectionItem> OnSelected;
        
        public TankData TankData { get; private set; }
        
        private Outline _outline;
        private bool _isSelected;
        
        public void Init(TankData tankData)
        {
            _outline = GetComponent<Outline>();
            TankData = tankData;
            _tankImage.sprite = TankData.LockedSprite;
            _tankName.text = TankData.Name;
        }

        public GameObject GetPreviewTankPrefab()
        {
            return TankData.PreviewPrefab;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;
        }

        public void SetOutlineEnabled(bool isEnabled)
        {
            _outline.enabled = isEnabled;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _outline.effectColor = _activeColor;
            _outline.enabled = true;
            
            _isSelected = true;
            OnSelected?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            
            _outline.effectColor = _hoverColor;
            _outline.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            
            _outline.enabled = false;
        }
    }
}