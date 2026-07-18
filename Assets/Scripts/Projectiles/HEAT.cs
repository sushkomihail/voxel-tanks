using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;
using UpgradeSystem;
using Screen = ArmorSystem.Screen;

namespace Projectiles
{
    public sealed class HEAT : Projectile
    {
        public override ProjectileType Type => ProjectileType.HEAT;
        
        public HEAT(ProjectileProps props, UpgradeBroker upgradeBroker, object owner,
            Vector3 position, Vector3 direction) : base(props, upgradeBroker, owner, position, direction)
        {
        }

        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            IsInactive = true;
        }

        protected override void HandleArmorHit(Armor armor, RaycastHit hit, Vector3 hitDirection)
        {
            float realPenetration = CalculateRealPenetration();
            float ricochetAngle = GlobalSettings.RicochetAngles[Type];
            
            if (armor.IsRicochet(hit.normal, hitDirection, _baseNormalization, ricochetAngle, _props.Caliber,
                    out float hitAngle))
            {
                OnRicochet(hit);
                return;
            }

            float reducedThickness = armor.GetReducedThickness(hitAngle);

            if (armor is Screen screen && screen.Module)
            {
                screen.TakeDamage(ModuleDamage, _owner);
                IsInactive = true;
                return;
            }
            
            TryDealDamageToArmor(armor, reducedThickness, realPenetration);
            IsInactive = true;
        }
    }
}