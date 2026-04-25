using Databases;
using SaveSystem;
using SaveSystem.SavableStructures;
using Tank;
using UnityEngine;

namespace Spawners
{
    public class PlayerTankSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private TanksDatabase _tanksDatabase;

        private PlayerTankController _prefab;

        public void Initialize()
        {
            BattleData data = Saver<BattleData>.Load(nameof(BattleData));
            _prefab = _tanksDatabase.Tanks[data.TankId].PlayerPrefab;
        }

        public PlayerTankController Spawn(TankCamera camera)
        {
            if (!_prefab) return null;
            
            PlayerTankController player = Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
            player.Initialize(camera);
            return player;
        }
    }
}