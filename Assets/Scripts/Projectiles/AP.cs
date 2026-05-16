using Settings;
using ShootingSystems;

namespace Projectiles
{
    public class AP : Projectile
    {
        public override void Initialize(ProjectileProps props, ShootingSystem shootingSystem)
        {
            base.Initialize(props, shootingSystem);
            Type = ProjectileType.AP;
            _normalization = GlobalSettings.Normalizations[Type];
            _props = props;
        }
    }
}