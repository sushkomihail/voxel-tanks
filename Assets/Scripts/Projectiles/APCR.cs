using Settings;
using ShootingSystems;

namespace Projectiles
{
    public class APCR : Projectile
    {
        public override ProjectileType Type => ProjectileType.APCR;

        public override void Initialize(ProjectileProps props, ShootingSystem shootingSystem)
        {
            base.Initialize(props, shootingSystem);
            _normalization = GlobalSettings.Normalizations[Type];
        }
    }
}