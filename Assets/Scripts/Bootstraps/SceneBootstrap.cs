using System.Collections.Generic;
using Spawners;
using Tank;
using UI;
using UnityEngine;

namespace Bootstraps
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private TankCamera _tankCamera;
        [SerializeField] private PlayerTankSpawner _playerTankSpawner;
        [SerializeField] private NPCTanksSpawner _npcTanksSpawner;
        [SerializeField] private UILoader _uiLoader;

        private PlayerTank _playerTank;
        private List<AITank> _npcTanks;
        
        private void Awake()
        {
            _playerTankSpawner.Initialize();
            _npcTanksSpawner.Initialize();
            
            SpawnPlayerTank();
            SpawnNpcTanks();

            _uiLoader.Initialize(_playerTank, _npcTanks, _tankCamera);
        }

        private void SpawnPlayerTank()
        {
            if (!_playerTankSpawner) return;
            
            _playerTank = _playerTankSpawner.Spawn(_tankCamera);
        }

        private void SpawnNpcTanks()
        {
            if (!_npcTanksSpawner || !_playerTank) return;

            _npcTanks = _npcTanksSpawner.Spawn(_playerTank, _tankCamera);
        }
    }
}