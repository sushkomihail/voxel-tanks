using System;
using System.Collections.Generic;
using AI;
using Environment.Base;
using Environment.Brick;
using Environment.Map;
using Input;
using Spawners;
using Tank;
using UI;
using UnityEngine;

namespace Scenes
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private TankCamera _tankCamera;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private Pathfinder _pathfinder;
        [SerializeField] private PlayerTankSpawner _playerTankSpawner;
        [SerializeField] private NPCTanksSpawner _npcTanksSpawner;
        [SerializeField] private UILoader _uiLoader;

        private PlayerTank _playerTank;
        private List<AITank> _npcTanks;
        private readonly List<Tank.Tank> _onFieldTanks = new();
        private readonly List<IRouterTarget> _routerTargets = new();
        private readonly List<IDisposable> _disposables = new();
        
        private void Awake()
        {
            _tankCamera.Initialize();
            
            _pathfinder.Initialize();
            
            _playerTankSpawner.Initialize();
            _npcTanksSpawner.Initialize(_pathfinder);
            
            GenerateMap();
            
            SpawnPlayerTank();
            SpawnNpcTanks();
            
            _onFieldTanks.Add(_playerTank);
            _onFieldTanks.AddRange(_npcTanks);
            
            // TODO: Make ai initialization from function
            _routerTargets.AddRange(_onFieldTanks);
            _routerTargets.AddRange(_mapGenerator.Bases);
            
            foreach (AITank npcTank in _npcTanks)
            {
                List<IRouterTarget> npcTargets = new(_routerTargets);
                npcTargets.Remove(npcTank);
                npcTank.Initialize(_pathfinder, npcTargets, _playerTank);
            }
            
            _routerTargets.Clear();
            
            InitializeBases();
            
            _uiLoader.Initialize(_playerTank, _npcTanks, _tankCamera);
        }

        private void OnEnable()
        {
            Brick.OnDestroyed += UpdateNavGrid;
        }

        private void OnDisable()
        {
            Brick.OnDestroyed -= UpdateNavGrid;
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        private void UpdateNavGrid()
        {
            _pathfinder.NavGrid.CreateCells();
        }

        private void GenerateMap()
        {
            if (_mapGenerator.transform.childCount > 0)
            {
                _mapGenerator.ClearMap();
            }
            
            _mapGenerator.Generate();
        }

        private void InitializeBases()
        {
            for (int i = 0; i < _mapGenerator.Bases.Count; i++)
            {
                BaseModel model = _mapGenerator.Bases[i];
                BaseView view = _uiLoader.InstantiateBaseView(i);
                
                model.Initialize(_onFieldTanks, _playerTank.BattleData.Id);
                
                _disposables.Add(new BasePresenter(model, view));
            }
        }

        private void SpawnPlayerTank()
        {
            if (!_playerTankSpawner) return;
            
            _playerTank = _playerTankSpawner.Spawn(_tankCamera);
        }

        private void SpawnNpcTanks()
        {
            if (!_npcTanksSpawner || !_playerTank) return;

            _npcTanks = _npcTanksSpawner.Spawn();
        }
    }
}