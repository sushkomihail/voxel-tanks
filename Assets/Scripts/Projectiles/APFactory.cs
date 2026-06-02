using UnityEngine;

namespace Projectiles
{
    public class APFactory : ProjectileFactory
    {
        public APFactory(ProjectileProps projectileProps) : base(projectileProps)
        {
        }

        public override Projectile CreateProjectile(Vector3 position, Vector3 direction)
        {
            return new AP(_projectileProps, position, direction);
        }
    }
}