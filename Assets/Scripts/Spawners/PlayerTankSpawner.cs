using Databases;
using SaveSystem;
using SaveSystem.SavableStructures;
using Tank;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Spawners
{
    public class PlayerTankSpawner : TankSpawner
    {
        private readonly BattleData _battleData;
        
        public PlayerTankSpawner(TanksDatabase tanksDatabase, IObjectResolver resolver)
            : base(tanksDatabase, resolver)
        {
            _battleData = Saver<BattleData>.Load(nameof(BattleData));
        }

        public override bool TrySpawn(Transform spawnPoint, out TankController controller)
        {
            TankController prefab = _tanksDatabase.Tanks[_battleData.TankId].Prefab;

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