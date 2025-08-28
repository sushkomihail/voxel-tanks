using Extensions;
using UnityEngine;

namespace Environment.Map
{
    [CreateAssetMenu(fileName = "MapLegend", menuName = "Map Legend")]
    public class MapLegend : ScriptableObject
    {
        [SerializeField] private ColorBlockMatch[] _colorPrefabMatches;

        public bool TryGetBlockPrefab(Color color, out BlockType type, out GameObject prefab)
        {
            foreach (var match in _colorPrefabMatches)
            {
                if (match.Color.IsEqualWithTolerance(color, 0.01f))
                {
                    type = match.BlockType;
                    prefab = match.BlockPrefab;
                    return true;
                }
            }
            
            type = default;
            prefab = null;
            return false;
        }

        public bool TryGetBlockColor(BlockType blockType, out Color color)
        {
            foreach (var match in _colorPrefabMatches)
            {
                if (match.BlockType == blockType)
                {
                    color = match.Color;
                    return true;
                }
            }
            
            color = default;
            return false;
        }
    }
}