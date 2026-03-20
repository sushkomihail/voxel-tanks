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
        [SerializeField] private Pathfinder _pathfinder;
        [SerializeField] private TanksDatabase _tanksDatabase;
        
        public void Initialize()
        {
            _pathfinder.Initialize();
        }
        
        public List<AITank> Spawn(PlayerTank playerTank, TankCamera camera)
        {
            var spawnedNpcTanks = new List<AITank>();
            
            foreach (var point in _spawnPoints)
            {
                AITank npcPrefab = GetRandomPrefab();
            
                if (!npcPrefab) continue;
            
                AITank npc = Instantiate(npcPrefab, point.position, point.rotation);
                npc.Initialize(_pathfinder, playerTank.Center);
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