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

        public TankSpawner(TanksDatabase tanksDatabase, IObjectResolver resolver)
        {
            _tanksDatabase = tanksDatabase;
            _resolver = resolver;
        }
        
        public abstract TankController Spawn(Transform spawnPoint);
    }
}