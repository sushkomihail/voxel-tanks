using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public sealed class HEAT : Projectile
    {
        public override ProjectileType Type => ProjectileType.HEAT;
        
        public HEAT(ProjectileProps props, Vector3 position, Vector3 direction) : base(props, position, direction)
        {
        }

        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            IsInactive = true;
        }

        protected override void HandleArmorHit(Armor armor, RaycastHit hit, Vector3 hitDirection)
        {
            float realPenetration = CalculateRealPenetration();
            float reducedThickness;

            if (CheckForThreeCaliberRule(armor))
            {
                reducedThickness = armor.GetReducedThickness(hit.normal, hitDirection, _baseNormalization);

                if (armor.IsScreen) IsInactive = true;
                else TryDealDamageToArmor(armor, reducedThickness, realPenetration);
                
                return;
            }

            float ricochetAngle = GlobalSettings.RicochetAngles[Type];

            if (Armor.IsRicochet(hit.normal, hitDirection, _baseNormalization, ricochetAngle,
                    out float hitAngle))
            {
                OnRicochet(hit);
                return;
            }

            reducedThickness = armor.GetReducedThickness(hitAngle);
            TryDealDamageToArmor(armor, reducedThickness, realPenetration);
        }
    }
}