using System;
using System.Collections.Generic;
using Environment.Base;
using Environment.Brick;
using Environment.Map;
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

        private MapGenerator _mapGenerator;
        private IRouterTargetRegistry _routerTargetRegistry;
        private PlayerSpawner _playerSpawner;
        private NPCSpawner _npcSpawner;
        private Pathfinder _pathfinder;
        private UILoader _uiLoader;
        private PlayerController _playerController;
        private readonly List<NPCController> _npcControllers = new();
        private readonly List<TankController> _onFieldTanks = new();
        private readonly List<IDisposable> _disposables = new();

        [Inject]
        public void Construct(
            MapGenerator mapGenerator,
            IRouterTargetRegistry routerTargetRegistry,
            PlayerSpawner playerSpawner,
            NPCSpawner npcSpawner,
            Pathfinder pathfinder,
            UILoader uiLoader)
        {
            _mapGenerator = mapGenerator;
            _routerTargetRegistry = routerTargetRegistry;
            _playerSpawner = playerSpawner;
            _npcSpawner = npcSpawner;
            _pathfinder = pathfinder;
            _uiLoader = uiLoader;
        }

        private void Awake()
        {
            _pathfinder.Initialize();
        }

        private void Start()
        {
            GenerateMap();
            
            SpawnPlayerTank();
            SpawnNpcTanks();
            _uiLoader.Initialize(_playerController);
            
            _onFieldTanks.Add(_playerController);
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
                
                model.Initialize(_onFieldTanks, _playerController);
                
                _disposables.Add(new BasePresenter(model, view));
            }
        }

        private void SpawnPlayerTank()
        {
            if (_spawnPoints.Count == 0 || _spawnPoints == null) return;
            
            int index = GetRandomSpawnPoint(out Transform point);
            _spawnPoints.RemoveAt(index);
            
            _playerController = (PlayerController)_playerSpawner.Spawn(point);
            _playerController.Initialize();
            
            _routerTargetRegistry.Register(_playerController);
        }

        private void SpawnNpcTanks()
        {
            if (_spawnPoints.Count == 0 || _spawnPoints == null) return;
            
            if (!_playerController) return;

            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                int index = GetRandomSpawnPoint(out Transform point);
                _spawnPoints.RemoveAt(index);

                NPCController npcController = (NPCController)_npcSpawner.Spawn(point);
                npcController.Initialize();
                
                _routerTargetRegistry.Register(npcController);
                
                _npcControllers.Add(npcController);
            }
        }

        private int GetRandomSpawnPoint(out Transform point)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Count);
            point = _spawnPoints[randomIndex];
            return randomIndex;
        }
    }
}