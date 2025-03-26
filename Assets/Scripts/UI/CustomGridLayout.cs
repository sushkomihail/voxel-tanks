using UnityEditor;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class CustomGridLayout : MonoBehaviour
    {
        [SerializeField] private int _columnsCount;
        [SerializeField] private int _rowsCount;
        [SerializeField] private Vector2 _itemSize;
        [SerializeField] private float _spacing;

        private RectTransform _rectTransform;

        [ContextMenu("Initialize")]
        public void Initialize()
        {
            Undo.RecordObject(transform, "GridLayout");
            int itemsCount = 3;
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            int createdItemsCount = 0;
            float startX = 
                (_rectTransform.sizeDelta.x - _itemSize.x * _columnsCount - _spacing * (_columnsCount - 1)) / 2;
            float startY = (_rectTransform.sizeDelta.y - _itemSize.y * _rowsCount - _spacing * (_rowsCount - 1)) / 2;
            Vector2 itemPosition = new Vector2(startX, startY);
                
            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _columnsCount; j++)
                {
                    if (createdItemsCount == itemsCount) return;
                    
                    CreateItem(itemPosition);
                    createdItemsCount++;
                    itemPosition.x += _itemSize.x + _spacing;
                }

                itemPosition.x = startX;
                itemPosition.y += _itemSize.y + _spacing;
            }
        }

        private void CreateItem(Vector2 position)
        {
            var item = new GameObject();
            item.transform.SetParent(transform);
            
            var itemRect = item.AddComponent<RectTransform>();
            itemRect.pivot = Vector2.zero;
            itemRect.anchorMin = Vector2.zero;
            itemRect.anchorMax = Vector2.zero;
            itemRect.anchoredPosition = position;
            itemRect.sizeDelta = _itemSize;
        }
    }
}
