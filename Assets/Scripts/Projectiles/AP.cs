using System;
using System.Collections.Generic;
using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public class AP : Projectile
    {
        public override ProjectileType Type => ProjectileType.AP;

        private const float FlightDistanceAfterScreenHitInCalibers = 10f;
        
        private readonly RaycastHit[] _behindScreenHits = new RaycastHit[5];
        private readonly DistanceComparer _distanceComparer = new();
        
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
            float ricochetAngle = GlobalSettings.RicochetAngles[Type];

            if (armor.IsRicochet(hit.normal, hitDirection, _baseNormalization, ricochetAngle, _props.Caliber,
                    out float hitAngle))
            {
                OnRicochet(hit);
                return;
            }
            
            float reducedThickness = armor.GetReducedThickness(hitAngle);

            if (armor.IsScreen)
            {
                OnScreenArmorHit(hit, hitDirection, reducedThickness, realPenetration);
            }
            else
            {
                TryDealDamageToArmor(armor, reducedThickness, realPenetration);
                IsInactive = true;
            }
        }

        private void OnScreenArmorHit(RaycastHit hit, Vector3 hitDirection, float reducedThickness, float realPenetration)
        {
            if (IsInactive) return;
            
            if (reducedThickness <= realPenetration)
            {
                _basePenetration -= reducedThickness;
                
                Vector3 behindScreenPosition = hit.point - hit.normal * 0.01f;
                Ray ray = new Ray(behindScreenPosition, hitDirection);
                float rayDistance = _props.Caliber / 1000f * FlightDistanceAfterScreenHitInCalibers;
                int hitCount = Physics.RaycastNonAlloc(ray, _behindScreenHits, rayDistance, _props.ArmorMask.value);

                if (hitCount > 0)
                {
                    Array.Sort(_behindScreenHits, 0, hitCount, _distanceComparer);

                    for (int i = 0; i < hitCount; i++)
                    {
                        if (!_behindScreenHits[i].collider.TryGetComponent(out Armor armor)) continue;

                        if (armor.IsScreen)
                        {
                            OnScreenArmorHit(_behindScreenHits[i], hitDirection, reducedThickness, realPenetration);
                        }
                        else
                        {
                            HandleArmorHit(armor, _behindScreenHits[i], hitDirection);
                        }
                    }
                }
            }
            else
            {
                IsInactive = true;
            }
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