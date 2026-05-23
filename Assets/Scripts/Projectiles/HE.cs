using Settings;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public class HE : Projectile
    {
        public override ProjectileType Type => ProjectileType.HE;

        private readonly Collider[] _affectedColliders = new Collider[10];

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            int hitCount = Physics.OverlapSphereNonAlloc(contact.point, _props.SplashRadius, _affectedColliders);

            for (int i = 0; i < hitCount; i++)
            {
                if (!_affectedColliders[i].TryGetComponent(out IDamageable damageable)) continue;
                
                if (damageable is Armor.Armor armor)
                {
                    float penetrationRatio =
                        1 + Random.Range(-GlobalSettings.PenetrationError, GlobalSettings.PenetrationError);
                    float realPenetration = _props.Penetration * penetrationRatio;
                    float damageRatio = Mathf.Clamp01(realPenetration / armor.Thickness);
                    armor.TakeDamage(new ArmorDamageDecorator(_props, damageRatio));
                }
                else
                {
                    damageable.TakeDamage(_props);
                }
                
            }
            
            _shootingSystem.OnProjectileHit(this);
        }
    }
}