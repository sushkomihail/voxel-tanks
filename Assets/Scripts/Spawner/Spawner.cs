using AI;
using Databases;
using SaveSystem;
using SaveSystem.SavableStructures;
using Tank;
using Tank.AI;
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
        
        [Space(5), Header("UI")]
        [SerializeField] private TankHealthView _tankHealthView;
        
        private void Awake()
        {
            Spawn();
        }

        private void Spawn()
        {
            PlayerTank player = SpawnPlayer();
            _tankHealthView?.Init(player.GetHealth().MaxHealth);
            player.GetHealth().SetView(_tankHealthView);
            
            if (!player) return;
            
            foreach (var point in _npcSpawnPoints)
            {
                SpawnNPC(point, player.transform);
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
    }
}