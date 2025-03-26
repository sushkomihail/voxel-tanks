using UnityEngine;
using Utils;

namespace ShootingSystems
{
    public abstract class ShootingSystem : MonoBehaviour
    {
        [SerializeField] protected Transform _projectilePivot;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private ProjectileData _projectileData;
        
        protected ObjectPool<Projectile> _projectilePool;
        
        private const int ProjectilesPoolDepth = 5;

        public void Initialize()
        {
            InitializeProjectilePool();
        }

        public float GetProjectileSpeed()
        {
            return _projectileData.FlightSpeed;
        }

        public abstract void Shoot();

        public void OnProjectileHit(Projectile projectile)
        {
            _projectilePool.Release(projectile);
        }

        private void InitializeProjectilePool()
        {
            _projectilePool = new ObjectPool<Projectile>(_projectile, InitializeProjectile, ProjectilesPoolDepth);
        }

        private void InitializeProjectile(Projectile projectile)
        {
            projectile.Initialize(_projectileData, this);
        }
    }
}