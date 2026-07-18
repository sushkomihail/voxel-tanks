using UnityEngine;
using UpgradeSystem;

namespace Projectiles
{
    public class HEATFactory : ProjectileFactory
    {
        public HEATFactory(ProjectileProps projectileProps, UpgradeBroker upgradeBroker, object owner)
            : base(projectileProps, upgradeBroker, owner)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new HEAT(_projectileProps, _upgradeBroker, _owner, position, direction);
        }
    }
}