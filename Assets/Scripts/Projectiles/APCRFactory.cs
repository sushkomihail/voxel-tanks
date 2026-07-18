using UnityEngine;
using UpgradeSystem;

namespace Projectiles
{
    public class APCRFactory : ProjectileFactory
    {
        public APCRFactory(ProjectileProps projectileProps, UpgradeBroker upgradeBroker, object owner)
            : base(projectileProps, upgradeBroker, owner)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new APCR(_projectileProps, _upgradeBroker, _owner, position, direction);
        }
    }
}