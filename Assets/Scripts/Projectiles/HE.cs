using System.Collections.Generic;
using ArmorSystem;
using ShootingSystems;
using Tank;
using UnityEngine;

namespace Projectiles
{
    public sealed class HE : Projectile
    {
        public override ProjectileType Type => ProjectileType.HE;

        private const int ShardRaysNumber = 32;
        private readonly Dictionary<TankController, int> _tankDamages = new();
        private Vector3[] _shardRayDirections;
        
        public HE(ProjectileProps props, Vector3 position, Vector3 direction) : base(props, position, direction)
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
            float reducedThickness =
                armor.GetReducedThickness(hit.normal, hitDirection, 0f);

            if (armor.IsScreen)
            {
                OnScreenArmorHit(hit, reducedThickness, realPenetration);
            }
            else
            {
                if (!TryDealDamageToArmor(armor, reducedThickness, realPenetration))
                {
                    Vector3 safeOrigin = hit.point + hit.normal * 0.01f;
                    EmitSplashRays(safeOrigin);
                    IsInactive = true;
                }
            }
        }

        private void OnScreenArmorHit(RaycastHit hit, float reducedThickness, float realPenetration)
        {
            if (reducedThickness <= realPenetration)
            {
                _basePenetration -= reducedThickness;
            }
            else
            {
                Vector3 safeOrigin = hit.point + hit.normal * 0.01f;
                EmitSplashRays(safeOrigin);
                IsInactive = true;
            }
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

        private int CalculateRealDamage(int armorThickness, float hitDistance)
        {
            float distanceRate = hitDistance / _props.SplashRadius;
            int damage = (int)(_props.ArmorDamage * (1 - distanceRate) / 2f - armorThickness * 1.3f);

            if (damage < 0) return 0;
            return damage;
        }

        private void EmitSplashRays(Vector3 origin)
        {
            _tankDamages.Clear();
            
            for (int i = 0; i < ShardRaysNumber; i++)
            {
#if UNITY_EDITOR
                Debug.DrawRay(origin, _shardRayDirections[i].normalized * _props.SplashRadius, Color.red, 5f);
#endif
                
                if (!Physics.Raycast(origin, _shardRayDirections[i], out RaycastHit hit, _props.SplashRadius)) continue;
                
                if (!hit.collider.TryGetComponent(out IDamageable damageable)) continue;

                if (damageable is Armor armor)
                {
                    if (armor.IsScreen) continue;
                    
                    if (hit.transform.root.TryGetComponent(out TankController tank))
                    {
                        int realDamage = CalculateRealDamage(armor.Thickness, hit.distance);
                            
                        if (!_tankDamages.TryGetValue(tank, out int currentDamage) || realDamage > currentDamage)
                        {
                            _tankDamages[tank] = realDamage;
                        }
                    }
                }
                else
                {
                    damageable.TakeDamage(_props);
                }
            }
            
            foreach ((TankController tank, int damage) in _tankDamages)
            {
                tank.Health.OnArmorDamaged(damage);
            }
        }
    }
}