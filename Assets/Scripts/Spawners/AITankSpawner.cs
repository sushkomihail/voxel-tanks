using Databases;
using Tank;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Spawners
{
    public class AITankSpawner : TankSpawner
    {
        public AITankSpawner(TanksDatabase tanksDatabase, IObjectResolver resolver) : base(tanksDatabase, resolver)
        {
        }

        public override bool TrySpawn(Transform spawnPoint, out TankController controller)
        {
            int randomIndex = Random.Range(0, _tanksDatabase.Tanks.Length);
            TankController prefab = _tanksDatabase.Tanks[randomIndex].Prefab;

            if (!prefab)
            {
                controller = null;
                return false;
            }
            
            controller = _resolver.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            return true;
        }
    }
}