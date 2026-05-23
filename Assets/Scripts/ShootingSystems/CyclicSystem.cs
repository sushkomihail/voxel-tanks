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
        
        public override void Shoot()
        {
            if (_elapsedReloadingTime < _systemData.ReloadingTime) return;

            var projectile = _projectilePools[LoadProjectile()].Get();
            projectile.Launch(_projectilePivot);
            _elapsedReloadingTime = 0;
        }
    }
}