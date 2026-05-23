using Settings;
using ShootingSystems;

namespace Projectiles
{
    public class AP : Projectile
    {
        public override ProjectileType Type => ProjectileType.AP;

        public override void Initialize(ProjectileProps props, ShootingSystem shootingSystem)
        {
            base.Initialize(props, shootingSystem);
            _normalization = GlobalSettings.Normalizations[Type];
            _props = props;
        }
    }
}