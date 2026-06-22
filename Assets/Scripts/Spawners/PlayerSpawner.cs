using Databases;
using Navigation;
using SaveSystem;
using SaveSystem.SavableStructures;
using Tank;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Spawners
{
    public class PlayerSpawner : TankSpawner
    {
        private readonly BattleData _battleData;
        
        public PlayerSpawner(TanksDatabase tanksDatabase, IObjectResolver resolver)
            : base(tanksDatabase, resolver)
        {
            _battleData = Saver<BattleData>.Load(nameof(BattleData));
        }

        public override TankController Spawn(Transform spawnPoint)
        {
            PlayerController prefab = _tanksDatabase.Tanks[_battleData.TankId].PlayerPrefab;
            
            if (!prefab) return null;
            
            return _resolver.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}