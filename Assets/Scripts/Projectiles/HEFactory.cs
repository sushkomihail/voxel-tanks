using UnityEngine;
using UpgradeSystem;

namespace Projectiles
{
    public class HEFactory : ProjectileFactory
    {
        public HEFactory(ProjectileProps projectileProps, UpgradeBroker upgradeBroker, object owner)
            : base(projectileProps, upgradeBroker, owner)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new HE(_projectileProps, _upgradeBroker, _owner, position, direction);
        }
    }
}