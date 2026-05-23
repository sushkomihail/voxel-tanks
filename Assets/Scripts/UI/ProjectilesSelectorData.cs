using ShootingSystems;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "UI/ProjectilesSelectorData", fileName = "ProjectilesSelectorData")]
    public class ProjectilesSelectorData : ScriptableObject
    {
        [SerializeField] private ProjectilesSelectorItemData[] _itemsData;

        public bool TryGetItemDataByProjectileType(ProjectileType type, out ProjectilesSelectorItemData data)
        {
            foreach (ProjectilesSelectorItemData itemData in _itemsData)
            {
                if (itemData.Type == type)
                {
                    data = itemData;
                    return true;
                } 
            }
            
            data = null;
            return false;
        }
    }
}