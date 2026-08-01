using System;
using System.Collections.Generic;
using Environment.Base;
using Environment.Brick;
using Environment.Map;
using EquipmentSystem;
using Navigation;
using Spawners;
using Tank;
using UI;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Scenes
{
    public class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;

        private GameInput _inputActions;
        private MapGenerator _mapGenerator;
        private TankCamera _tankCamera;
        private IRouterTargetRegistry _routerTargetRegistry;
        private PlayerTankSpawner _playerTankSpawner;
        private AITankSpawner _aiTankSpawner;
        private Pathfinder _pathfinder;
        private EquipmentPresenter _equipmentPresenter;
        private UILoader _uiLoader;
        
        private TankController _playerTankController;
        private readonly List<TankController> _npcControllers = new();
        private readonly List<TankController> _onFieldTanks = new();
        private readonly List<IDisposable> _disposables = new();

        [Inject]
        public void Construct(
            GameInput inputActions,
            MapGenerator mapGenerator,
            TankCamera tankCamera,
            IRouterTargetRegistry routerTargetRegistry,
            PlayerTankSpawner playerTankSpawner,
            AITankSpawner aiTankSpawner,
            Pathfinder pathfinder,
            EquipmentPresenter equipmentPresenter,
            UILoader uiLoader)
        {
            _inputActions = inputActions;
            _mapGenerator = mapGenerator;
            _tankCamera = tankCamera;
            _routerTargetRegistry = routerTargetRegistry;
            _playerTankSpawner = playerTankSpawner;
            _aiTankSpawner = aiTankSpawner;
            _pathfinder = pathfinder;
            _equipmentPresenter = equipmentPresenter;
            _uiLoader = uiLoader;
        }

        private void Awake()
        {
            _pathfinder.Initialize();
        }

        private void Start()
        {
            GenerateMap();
            
            var spawnPointsCopy = new List<Transform>(_spawnPoints);
            SpawnPlayerTank(spawnPointsCopy);
            SpawnNpcTanks(spawnPointsCopy);
            
            _uiLoader.Initialize(_playerTankController);
            
            _onFieldTanks.Add(_playerTankController);
            _onFieldTanks.AddRange(_npcControllers);
            
            InitializeBases();
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
                
                model.Initialize(_onFieldTanks, _playerTankController);
                
                _disposables.Add(new BasePresenter(model, view));
            }
        }

        private void SpawnPlayerTank(List<Transform> spawnPoints)
        {
            if (spawnPoints == null || spawnPoints.Count == 0) return;
            
            int index = GetRandomSpawnPoint(spawnPoints, out Transform point);
            spawnPoints.RemoveAt(index);

            if (_playerTankSpawner.TrySpawn(point, out _playerTankController))
            {
                _playerTankController.Initialize(new PlayerTankConfigurator(
                    _playerTankController,
                    _inputActions,
                    _tankCamera,
                    _equipmentPresenter));
                
                _routerTargetRegistry.Register(_playerTankController);
            }
        }

        private void SpawnNpcTanks(List<Transform> spawnPoints)
        {
            if (spawnPoints == null || spawnPoints.Count == 0) return; 
            
            if (!_playerTankController) return;

            while (spawnPoints.Count > 0) 
            { 
                int index = GetRandomSpawnPoint(spawnPoints, out Transform point); 
                spawnPoints.RemoveAt(index);

                if (_aiTankSpawner.TrySpawn(point, out var aiTankController))
                {
                    aiTankController.Initialize(new AITankConfigurator(aiTankController));
                    
                    _routerTargetRegistry.Register(aiTankController); 
                    _npcControllers.Add(aiTankController); 
                }
            } 
        }

        private static int GetRandomSpawnPoint(List<Transform> spawnPoints, out Transform point)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            point = spawnPoints[randomIndex];
            return randomIndex;
        }
    }
}