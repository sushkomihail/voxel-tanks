using Databases;
using Tank;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Spawners
{
    public class NPCSpawner : TankSpawner
    {
        public NPCSpawner(TanksDatabase tanksDatabase, IObjectResolver resolver) : base(tanksDatabase, resolver)
        {
        }

        public override TankController Spawn(Transform spawnPoint)
        {
            int randomIndex = Random.Range(0, _tanksDatabase.Tanks.Length);
            NPCController prefab = _tanksDatabase.Tanks[randomIndex].NpcPrefab;
            
            if (!prefab) return null;
            
            return _resolver.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}