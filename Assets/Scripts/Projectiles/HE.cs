using System.Collections.Generic;
using ArmorSystem;
using ShootingSystems;
using UnityEngine;
using UpgradeSystem;
using Screen = ArmorSystem.Screen;

namespace Projectiles
{
    public sealed class HE : Projectile
    {
        public override ProjectileType Type => ProjectileType.HE;

        private const int ShardRaysNumber = 32;
        
        private readonly Dictionary<IDamageable, int> _damages = new();
        private Vector3[] _shardRayDirections;
        
        public HE(ProjectileProps props, UpgradeBroker upgradeBroker, object owner,
            Vector3 position, Vector3 direction) : base(props, upgradeBroker, owner, position, direction)
        {
            InitializeShardRayDirections();
        }
        
        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            Vector3 safeOrigin = hit.point + hit.normal * 0.01f;
            EmitSplashRays(safeOrigin);
            IsInactive = true;
        }

        protected override void HandleArmorHit(Armor armor, RaycastHit hit, Vector3 hitDirection)
        {
            float realPenetration = CalculateRealPenetration();
            float reducedThickness = armor.GetReducedThickness(hit.normal, hitDirection, 0f, _props.Caliber);

            if (armor is Screen screen)
            {
                HandleScreenHit(hit, screen, reducedThickness, realPenetration);
            }
            else
            {
                if (!TryDealDamageToArmor(armor, reducedThickness, realPenetration))
                {
                    Vector3 safeOrigin = hit.point + hit.normal * 0.01f;
                    EmitSplashRays(safeOrigin);
                }
                
                IsInactive = true;
            }
        }

        private void HandleScreenHit(RaycastHit hit, Screen screen, float reducedThickness, float realPenetration)
        {
            if (reducedThickness > realPenetration)
            {
                Vector3 safeOrigin = hit.point + hit.normal * 0.01f;
                EmitSplashRays(safeOrigin);
                IsInactive = true;
                return;
            }
            
            screen.TakeDamage(ModuleDamage, _owner);
            _basePenetration -= reducedThickness;
        }

        private void InitializeShardRayDirections()
        {
            _shardRayDirections = new Vector3[ShardRaysNumber];

            for (int i = 0; i < ShardRaysNumber; i++)
            {
                float y = 1f - 2f * i / (ShardRaysNumber - 1f);
                float r = Mathf.Sqrt(1f - y * y);
                float goldenAngle = i * Mathf.PI * 0.763932f;

                float x = r * Mathf.Cos(goldenAngle);
                float z = r * Mathf.Sin(goldenAngle);

                _shardRayDirections[i] = new Vector3(x, y, z);
            }
        }

        private int CalculateRealDamage(int baseDamage, int armorThickness, float hitDistance)
        {
            float distanceRate = hitDistance / _props.SplashRadius;
            int damage = (int)(baseDamage * (1 - distanceRate) / 2f - armorThickness * 1.3f);

            if (damage < 0) return 0;
            return damage;
        }

        private void EmitSplashRays(Vector3 origin)
        {
            _damages.Clear();
            
            for (int i = 0; i < ShardRaysNumber; i++)
            {
                Vector3 direction = _shardRayDirections[i].normalized;
                Ray ray = new Ray(origin, direction);
                
                if (!Physics.Raycast(ray, out RaycastHit hit, _props.SplashRadius, _props.HitMask.value)) continue;
                
                HandleSplashRayHit(hit);
            }
            
            foreach ((IDamageable damageable, int damage) in _damages)
            {
                damageable.TakeDamage(damage, _owner);
            }
        }

        private void HandleSplashRayHit(RaycastHit hit)
        {
            if (!hit.collider.TryGetComponent(out IDamageable damageable)) return;

            (IDamageable damageTarget, int thickness, int damage) = damageable switch
            {
                Screen screen => (screen.Module, screen.Thickness, ModuleDamage),
                Armor armor => (armor.TankHealth, armor.Thickness, ArmorDamage),
                _ => ((IDamageable)null, 0, 0)
            };

            if (damageTarget != null)
            {
                int realDamage = CalculateRealDamage(damage, thickness, hit.distance);
                
                if (!_damages.TryGetValue(damageTarget, out int currentDamage) || realDamage > currentDamage)
                {
                    _damages[damageTarget] = realDamage;
                }
            }
            else if (damageable is not Screen)
            {
                damageable.TakeDamage(1, _owner);
            }
        }
    }
}