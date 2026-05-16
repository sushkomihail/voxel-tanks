using Settings;
using ShootingSystems;

namespace Projectiles
{
    public class APCR : Projectile
    {
        public override void Initialize(ProjectileProps props, ShootingSystem shootingSystem)
        {
            base.Initialize(props, shootingSystem);
            Type = ProjectileType.APCR;
            _normalization = GlobalSettings.Normalizations[Type];
        }
    }
}