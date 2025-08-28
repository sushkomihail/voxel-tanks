using System;
using UnityEngine;

namespace Environment.Map
{
    [Serializable]
    public class ColorBlockMatch
    {
        [SerializeField] private BlockType _blockType;
        [SerializeField] private Color _color;
        [SerializeField] private GameObject _blockPrefab;

        public BlockType BlockType => _blockType;
        public Color Color => _color;
        public GameObject BlockPrefab => _blockPrefab;
    }
}