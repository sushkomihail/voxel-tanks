using System.Collections.Generic;
using AI;
using Databases;
using Tank;
using UnityEngine;

namespace Spawners
{
    public class NPCTanksSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private TanksDatabase _tanksDatabase;
        
        private Pathfinder _pathfinder;
        
        public void Initialize(Pathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }
        
        public List<AITankController> Spawn()
        {
            var spawnedNpcTanks = new List<AITankController>();
            
            foreach (var point in _spawnPoints)
            {
                AITankController npcPrefab = GetRandomPrefab();
            
                if (!npcPrefab) continue;
            
                AITankController npc = Instantiate(npcPrefab, point.position, point.rotation);
                spawnedNpcTanks.Add(npc);
            }
            
            return spawnedNpcTanks;
        }

        private AITankController GetRandomPrefab()
        {
            int randomIndex = Random.Range(0, _tanksDatabase.Tanks.Length);
            return _tanksDatabase.Tanks[1].NpcPrefab;
        }
    }
}