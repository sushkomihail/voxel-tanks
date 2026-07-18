using Projectiles;
using ShootingSystems.Data;
using UnityEngine;
using UpgradeSystem;

namespace ShootingSystems
{
    public class CyclicSystem : ShootingSystem
    {
        [SerializeField] private CyclicSystemData _systemData;

        private float ReloadingTime
        {
            get
            {
                StatQuery query = new StatQuery(StatType.Reloading, _systemData.ReloadingTime);
                _upgradeBroker.Query(_owner, query);
                return query.Value;
            }
        }
        
        private float _elapsedReloadingTime;

        private void Update()
        {
            if (_elapsedReloadingTime >= ReloadingTime) return;

            _elapsedReloadingTime += Time.deltaTime;
        }

        public float GetReloadingRate()
        {
            if (ReloadingTime == 0) return 0;
            
            return _elapsedReloadingTime / ReloadingTime;
        }

        public override void SetNextProjectileTypeAsCurrent()
        {
            base.SetNextProjectileTypeAsCurrent();
            
            _elapsedReloadingTime = 0;
        }

        public override void Shoot()
        {
            if (_elapsedReloadingTime < ReloadingTime) return;

            ProjectilesUpdater.Instance.AddProjectile(CreateProjectile());
            _elapsedReloadingTime = 0;
        }
    }
}