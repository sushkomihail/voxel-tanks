using Databases;
using Tank;
using UnityEngine;
using VContainer;

namespace Spawners
{
    public abstract class TankSpawner
    {
        protected readonly TanksDatabase _tanksDatabase;
        protected readonly IObjectResolver _resolver;

        protected TankSpawner(TanksDatabase tanksDatabase, IObjectResolver resolver)
        {
            _tanksDatabase = tanksDatabase;
            _resolver = resolver;
        }
        
        public abstract bool TrySpawn(Transform spawnPoint, out TankController controller);
    }
}