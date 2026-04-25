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
        [SerializeField] private PlayerTankController _playerPrefab;
        [SerializeField] private AITankController _npcPrefab;
        
        public string Name => _name;
        public Sprite LockedSprite => _lockedSprite;
        public Sprite UnlockedSprite => _unlockedSprite;
        public GameObject PreviewPrefab => _previewPrefab;
        public PlayerTankController PlayerPrefab => _playerPrefab;
        public AITankController NpcPrefab => _npcPrefab;
    }
}