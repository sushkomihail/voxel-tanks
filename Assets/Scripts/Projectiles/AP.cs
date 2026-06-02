using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public class AP : Projectile
    {
        public override ProjectileType Type => ProjectileType.AP;
        
        public AP(ProjectileProps props, Vector3 position, Vector3 direction) : base(props, position, direction)
        {
            _baseNormalization = GlobalSettings.Normalizations[ProjectileType.AP];
        }

        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_props);
            }
            
            IsInactive = true;
        }

        protected override void HandleArmorHit(Armor armor, RaycastHit hit, Vector3 hitDirection)
        {
            float realPenetration = CalculateRealPenetration();
            float reducedThickness;

            if (CheckForThreeCaliberRule(armor))
            {
                reducedThickness = armor.GetReducedThickness(hit.normal, hitDirection, _baseNormalization);

                if (armor.IsScreen) OnScreenArmorHit(reducedThickness, realPenetration);
                else TryDealDamageToArmor(armor, reducedThickness, realPenetration);
                
                return;
            }

            float realNormalization = ApplyTwoCaliberRule(armor);
            float ricochetAngle = GlobalSettings.RicochetAngles[Type];

            if (Armor.IsRicochet(hit.normal, hitDirection, realNormalization, ricochetAngle,
                    out float hitAngle))
            {
                OnRicochet(hit);
                return;
            }

            reducedThickness = armor.GetReducedThickness(hitAngle);
            
            if (armor.IsScreen) OnScreenArmorHit(reducedThickness, realPenetration);
            else TryDealDamageToArmor(armor, reducedThickness, realPenetration);
        }

        private void OnScreenArmorHit(float reducedThickness, float realPenetration)
        {
            if (reducedThickness <= realPenetration)
            {
                _basePenetration -= reducedThickness;
            }
            else
            {
                IsInactive = true;
            }
        }
    }
}