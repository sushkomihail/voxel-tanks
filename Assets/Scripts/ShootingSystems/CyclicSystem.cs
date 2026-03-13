using UnityEngine;

namespace ShootingSystems
{
    public class CyclicSystem : ShootingSystem
    {
        [SerializeField] private float _reloadingTime;
        
        private float _elapsedReloadingTime;

        private void Update()
        {
            if (_elapsedReloadingTime >= _reloadingTime) return;

            _elapsedReloadingTime += Time.deltaTime;
        }

        public float GetReloadingRate()
        {
            return _elapsedReloadingTime / _reloadingTime;
        }
        
        public override void Shoot()
        {
            if (_elapsedReloadingTime < _reloadingTime) return;

            var projectile = _projectilePools[_selectedProjectileType].Get();
            projectile.Launch(_projectilePivot);
            _elapsedReloadingTime = 0;
        }
    }
}