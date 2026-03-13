using AI;
using Databases;
using SaveSystem;
using SaveSystem.SavableStructures;
using Tank;
using Tank.AI;
using UI.Aims;
using UnityEngine;

namespace Spawner
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _npcSpawnPoints;
        [SerializeField] private Transform _playerSpawnPoint;
     
        [Space(5), Header("External components")]
        [SerializeField] private TankCamera _tankCamera;
        [SerializeField] private Pathfinder _pathfinder;
        
        [Space(5), Header("Databases")]
        [SerializeField] private TanksDatabase _tanksDatabase;
        [SerializeField] private AimsDatabase _aimsDatabase;
        
        [Space(5), Header("UI")]
        [SerializeField] private Transform _canvas;
        [SerializeField] private TankHealthView _tankHealthView;
        
        private PlayerTank _playerTank;
        
        private void Awake()
        {
            Spawn();
            
            // TODO: Make ui initializer class
            InitUI();
        }

        private void Spawn()
        {
            _playerTank = SpawnPlayer();
            
            if (!_playerTank) return;
            
            foreach (var point in _npcSpawnPoints)
            {
                SpawnNPC(point, _playerTank.transform);
            }
        }

        private PlayerTank SpawnPlayer()
        {
            BattleData data = Saver<BattleData>.Load(nameof(BattleData));
            PlayerTank prefab = _tanksDatabase.Tanks[data.TankId].PlayerPrefab;
            
            if (!prefab) return null;
            
            PlayerTank player = Instantiate(prefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
            player.SetCamera(_tankCamera);
            return player;
        }
        
        private void SpawnNPC(Transform spawnPoint, Transform target)
        {
            int randomIndex = Random.Range(0, _tanksDatabase.Tanks.Length);
            AITank npcPrefab = _tanksDatabase.Tanks[randomIndex].NpcPrefab;
            
            if (!npcPrefab) return;
            
            AITank npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            npc.SetPathfinder(_pathfinder);
            npc.SetTarget(target);
        }

        private void InitUI()
        {
            if (!_playerTank) return;
            
            _tankHealthView?.Init(_playerTank.Health.MaxHealth);
            _playerTank.Health.SetView(_tankHealthView);

            Aim aimPrefab = _aimsDatabase.GetAimByShootingSystem(_playerTank.Gun.ShootingSystem);

            if (!aimPrefab) return;
            
            Aim aim = Instantiate(aimPrefab, _canvas);
            aim.Init(_playerTank.Gun, _tankCamera.Camera);
        }
    }
}