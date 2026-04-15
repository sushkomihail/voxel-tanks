using System.Collections.Generic;
using Environment.Base;
using Environment.Water;
using Extensions;
using UnityEngine;

namespace Environment.Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Texture2D _mapTexture;
        [SerializeField] private MapLegend _mapLegend;
        [SerializeField] private GameObject _groundPrefab;
        [SerializeField] private float _blockSize = 3f;
        
        public IReadOnlyList<BaseModel> Bases => _bases.AsReadOnly();
        
        private readonly List<BaseModel> _bases = new();

        [ContextMenu("Generate")]
        public void Generate()
        {
            for (int i = 0; i < _mapTexture.height; i++)
            {
                for (int j = 0; j < _mapTexture.width; j++)
                {
                    Color pixelColor = _mapTexture.GetPixel(j, i);
                    Vector3 position = new Vector3(j * _blockSize, 0, i * _blockSize);

                    if (_mapLegend.TryGetBlockPrefab(pixelColor, out BlockType type, out GameObject prefab))
                    {
                        if (prefab != null)
                        {
                            if (type == BlockType.Water)
                            {
                                InstantiateWaterBlock(prefab, position, new Vector2Int(j, i));
                            }
                            else
                            {
                                GameObject gameObject = Instantiate(prefab, position, Quaternion.identity, transform);

                                if (gameObject.TryGetComponent(out BaseModel baseModel))
                                {
                                    _bases.Add(baseModel);
                                }
                            }
                        }
                    }
                    
                    InstantiateGroundPrefab(position, type);
                }
            }
        }

        [ContextMenu("Clear Map")]
        public void ClearMap()
        {
            for (int i = transform.childCount - 1; i >= 0; i--) 
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private void InstantiateGroundPrefab(Vector3 position, BlockType aboveGroundBlockType)
        {
            if (_groundPrefab == null) return;
                    
            Quaternion rotation = GetRandomBlockRotation();
                    
            if (aboveGroundBlockType == BlockType.Water)
            {
                position.y -= _blockSize;
            }
                    
            Instantiate(_groundPrefab, position, rotation, transform);
        }

        private static Quaternion GetRandomBlockRotation()
        {
            int[] blockRotations = { 0, 90, -90, 180 };
            int index = Random.Range(0, blockRotations.Length);
            return Quaternion.Euler(0f, blockRotations[index], 0f);
        }

        private void InstantiateWaterBlock(GameObject prefab, Vector3 worldPosition, Vector2Int mapCoords)
        {
            GameObject obj = Instantiate(prefab, worldPosition, Quaternion.identity, transform);

            if (!_mapLegend.TryGetBlockColor(BlockType.Water, out Color color)) return;
            
            var checkDirections = new Dictionary<WaterBlockWallType, Vector2Int>
            {
                { WaterBlockWallType.Left, Vector2Int.left },
                { WaterBlockWallType.Right, Vector2Int.right },
                { WaterBlockWallType.Front, Vector2Int.up },
                { WaterBlockWallType.Back, Vector2Int.down }
            };

            foreach (var pair in checkDirections)
            {
                int x = mapCoords.x + pair.Value.x;
                int y = mapCoords.y + pair.Value.y;
                Color pixelColor = _mapTexture.GetPixel(x, y);

                if (!pixelColor.IsEqualWithTolerance(color, 0.01f)) continue;
                
                if (obj.TryGetComponent(out Water.Water water))
                {
                    water.DisableWall(pair.Key);
                }
            }
        }
    }
}