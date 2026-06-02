using UnityEngine;

namespace Projectiles
{
    public class APCRFactory : ProjectileFactory
    {
        public APCRFactory(ProjectileProps projectileProps) : base(projectileProps)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new APCR(_projectileProps, position, direction);
        }
    }
}