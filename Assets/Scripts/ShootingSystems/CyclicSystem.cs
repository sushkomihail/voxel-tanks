using Projectiles;
using ShootingSystems.Data;
using UnityEngine;

namespace ShootingSystems
{
    public class CyclicSystem : ShootingSystem
    {
        [SerializeField] private CyclicSystemData _systemData;
        
        private float _elapsedReloadingTime;

        private void Update()
        {
            if (_elapsedReloadingTime >= _systemData.ReloadingTime) return;

            _elapsedReloadingTime += Time.deltaTime;
        }

        public float GetReloadingRate()
        {
            if (_systemData.ReloadingTime == 0) return 0;
            
            return _elapsedReloadingTime / _systemData.ReloadingTime;
        }

        public override void SetNextProjectileTypeAsCurrent()
        {
            base.SetNextProjectileTypeAsCurrent();
            
            _elapsedReloadingTime = 0;
        }

        public override void Shoot()
        {
            if (_elapsedReloadingTime < _systemData.ReloadingTime) return;

            ProjectilesUpdater.Instance.AddProjectile(CreateProjectile());
            _elapsedReloadingTime = 0;
        }
    }
}