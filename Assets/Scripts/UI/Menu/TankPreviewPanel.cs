using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class TankPreviewPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 _startDragPosition;
        private Vector2 _dragVector;
        private Vector2 _lastDragVector;
        
        public Vector2 DragDelta { get; private set; }

        private void Update()
        {
            if (_dragVector == _lastDragVector)
            {
                DragDelta = Vector2.zero;
            }
            
            _lastDragVector = _dragVector;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startDragPosition = eventData.position;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _dragVector = eventData.position - _startDragPosition;
            DragDelta = eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragDelta = Vector2.zero;
        }
    }
}