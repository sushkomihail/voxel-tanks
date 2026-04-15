using System.Collections.Generic;
using AI;
using Databases;
using Input;
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
        
        public List<AITank> Spawn()
        {
            var spawnedNpcTanks = new List<AITank>();
            
            foreach (var point in _spawnPoints)
            {
                AITank npcPrefab = GetRandomPrefab();
            
                if (!npcPrefab) continue;
            
                AITank npc = Instantiate(npcPrefab, point.position, point.rotation);
                spawnedNpcTanks.Add(npc);
            }
            
            return spawnedNpcTanks;
        }

        private AITank GetRandomPrefab()
        {
            int randomIndex = Random.Range(0, _tanksDatabase.Tanks.Length);
            return _tanksDatabase.Tanks[1].NpcPrefab;
        }
    }
}