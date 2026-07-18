using UnityEngine;
using UpgradeSystem;

namespace Projectiles
{
    public abstract class ProjectileFactory
    {
        protected readonly ProjectileProps _projectileProps;
        protected readonly UpgradeBroker _upgradeBroker;
        protected readonly object _owner;

        protected ProjectileFactory(ProjectileProps projectileProps, UpgradeBroker upgradeBroker, object owner)
        {
            _projectileProps = projectileProps;
            _upgradeBroker = upgradeBroker;
            _owner = owner;
        }

        public abstract Projectile CreateProjectile(Vector3 position, Vector3 direction);
    }
}