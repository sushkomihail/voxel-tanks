using Projectiles;

namespace ShootingSystems
{
    public interface IDamageable
    {
        public void TakeDamage(ProjectileProps props);
    }
}