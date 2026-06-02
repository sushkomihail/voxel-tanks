using UnityEngine;

namespace Projectiles
{
    public class HEATFactory : ProjectileFactory
    {
        public HEATFactory(ProjectileProps projectileProps) : base(projectileProps)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new HEAT(_projectileProps, position, direction);
        }
    }
}