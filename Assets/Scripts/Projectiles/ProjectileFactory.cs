using UnityEngine;

namespace Projectiles
{
    public abstract class ProjectileFactory
    {
        protected readonly ProjectileProps _projectileProps;

        protected ProjectileFactory(ProjectileProps projectileProps)
        {
            _projectileProps = projectileProps;
        }

        public abstract Projectile CreateProjectile(Vector3 position, Vector3 direction);
    }
}