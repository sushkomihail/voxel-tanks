using System;
using System.Collections.Generic;
using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;
using UpgradeSystem;
using Screen = ArmorSystem.Screen;

namespace Projectiles
{
    public class AP : Projectile
    {
        public override ProjectileType Type => ProjectileType.AP;

        private const float FlightDistanceAfterScreenHitInCalibers = 10f;
        
        private readonly RaycastHit[] _behindScreenHits = new RaycastHit[5];
        private readonly DistanceComparer _distanceComparer = new();

        private float FlightDistanceAfterScreenHit => _props.Caliber / 1000f * FlightDistanceAfterScreenHitInCalibers;
        
        public AP(ProjectileProps props, UpgradeBroker upgradeBroker, object owner,
            Vector3 position, Vector3 direction) : base(props, upgradeBroker, owner, position, direction)
        {
            _baseNormalization = GlobalSettings.Normalizations[ProjectileType.AP];
        }

        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(ArmorDamage, _owner);
            }
            
            IsInactive = true;
        }

        protected override void HandleArmorHit(Armor armor, RaycastHit hit, Vector3 hitDirection)
        {
            float realPenetration = CalculateRealPenetration();
            float? reducedThickness = TryGetReducedThickness(armor, hit, hitDirection);
            
            if (reducedThickness is null) return;

            if (armor is Screen screen)
            {
                HandleScreenHit(screen, reducedThickness.Value, realPenetration);
                
                if (IsInactive) return;
                
                int hitCount = CountBehindScreenHits(hit, hitDirection);
                HandleBehindScreenHits(hitCount, hitDirection);
            }
            else
            {
                TryDealDamageToArmor(armor, reducedThickness.Value, realPenetration);
                IsInactive = true;
            }
        }

        private void HandleScreenHit(Screen screen, float reducedThickness, float realPenetration)
        {
            if (reducedThickness > realPenetration)
            {
                IsInactive = true;
                return;
            }
            
            screen.TakeDamage(ModuleDamage, _owner);
            _basePenetration -= reducedThickness;
        }

        private int CountBehindScreenHits(RaycastHit hit, Vector3 hitDirection)
        {
            Vector3 behindScreenPosition = hit.point - hit.normal * 0.01f;
            Ray ray = new Ray(behindScreenPosition, hitDirection);
            return Physics.RaycastNonAlloc(ray, _behindScreenHits, FlightDistanceAfterScreenHit, _props.ArmorMask.value);
        }

        private void HandleBehindScreenHits(int hitCount, Vector3 hitDirection)
        {
            if (hitCount == 0) return;
            
            Array.Sort(_behindScreenHits, 0, hitCount, _distanceComparer);

            for (int i = 0; i < hitCount; i++)
            {
                if (IsInactive) return;
                
                RaycastHit hit = _behindScreenHits[i];
                        
                if (!hit.collider.TryGetComponent(out Armor armor)) continue;
                
                float realPenetration = CalculateRealPenetration();
                float? reducedThickness = TryGetReducedThickness(armor, hit, hitDirection);
                
                if (reducedThickness is null) return;

                if (armor is Screen screen)
                {
                    HandleScreenHit(screen, reducedThickness.Value, realPenetration);
                }
                else
                {
                    TryDealDamageToArmor(armor, reducedThickness.Value, realPenetration);
                    IsInactive = true;
                }
            }
        }
        
        private float? TryGetReducedThickness(Armor armor, RaycastHit hit, Vector3 hitDirection)
        {
            float ricochetAngle = GlobalSettings.RicochetAngles[Type];
    
            if (armor.IsRicochet(hit.normal, hitDirection, _baseNormalization, ricochetAngle, _props.Caliber, out float hitAngle))
            {
                OnRicochet(hit);
                return null;
            }
    
            return armor.GetReducedThickness(hitAngle);
        }
        
        private class DistanceComparer : IComparer<RaycastHit>
        {
            public int Compare(RaycastHit x, RaycastHit y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }
    }
}