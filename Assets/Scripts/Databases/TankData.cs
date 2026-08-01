using System;
using Tank;
using UnityEngine;

namespace Databases
{
    [Serializable]
    public class TankData
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _lockedSprite;
        [SerializeField] private Sprite _unlockedSprite;
        [SerializeField] private GameObject _previewPrefab;
        [SerializeField] private TankController _prefab;
        
        public string Name => _name;
        public Sprite LockedSprite => _lockedSprite;
        public Sprite UnlockedSprite => _unlockedSprite;
        public GameObject PreviewPrefab => _previewPrefab;
        public TankController Prefab => _prefab;
    }
}