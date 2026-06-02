using UnityEngine;

namespace Projectiles
{
    public class HEFactory : ProjectileFactory
    {
        public HEFactory(ProjectileProps projectileProps) : base(projectileProps)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new HE(_projectileProps, position, direction);
        }
    }
}