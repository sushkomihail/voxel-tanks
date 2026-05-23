using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Outline))]
    public class ProjectilesSelectorItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _projectileImage;
        [SerializeField] private Color _selectedAsCurrentColor;
        [SerializeField] private Color _selectedAsNextColor;
        
        public ProjectilesSelectorItemData Data { get; private set; }
        public event Action<ProjectilesSelectorItem> OnSelected;
        
        private Outline _outline;
        public void Initialize(ProjectilesSelectorItemData data)
        {
            _outline = GetComponent<Outline>();
            Data = data;
            _projectileImage.sprite = Data.Sprite;
            _outline.enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }
        
        public void SelectAsCurrent()
        {
            _outline.enabled = true;
            _outline.effectColor = _selectedAsCurrentColor;
        }

        public void SelectAsNext()
        {
            _outline.enabled = true;
            _outline.effectColor = _selectedAsNextColor;
        }

        public void Deselect()
        {
            _outline.enabled = false;
        }
    }
}