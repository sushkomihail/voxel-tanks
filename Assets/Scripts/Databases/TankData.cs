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
        [SerializeField] private Tank.PlayerTank _playerPrefab;
        [SerializeField] private AITank _npcPrefab;
        
        public string Name => _name;
        public Sprite LockedSprite => _lockedSprite;
        public Sprite UnlockedSprite => _unlockedSprite;
        public GameObject PreviewPrefab => _previewPrefab;
        public Tank.PlayerTank PlayerPrefab => _playerPrefab;
        public AITank NpcPrefab => _npcPrefab;
    }
}