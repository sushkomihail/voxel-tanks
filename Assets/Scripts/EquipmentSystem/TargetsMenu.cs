using UnityEngine;

namespace EquipmentSystem
{
    public class TargetsMenu : MonoBehaviour
    {
        [SerializeField] private TargetsMenuItem _itemPrefab;
        [SerializeField] private Transform _container;

        public void Initialize(Sprite[] sprites, string[] bindings)
        {
            InstantiateItems(sprites, bindings);
        }

        private void InstantiateItems(Sprite[] sprites, string[] bindings)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                TargetsMenuItem item = Instantiate(_itemPrefab, _container);
                item.Initialize(sprites[i], bindings[i]);
            }
        }
    }
}