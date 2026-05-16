using ShootingSystems;

namespace Projectiles
{
    public class HEAT : Projectile
    {
        public override void Initialize(ProjectileProps props, ShootingSystem shootingSystem)
        {
            base.Initialize(props, shootingSystem);
            Type = ProjectileType.HEAT;
        }
    }
}