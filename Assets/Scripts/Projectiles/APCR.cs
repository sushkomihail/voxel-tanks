using Settings;
using ShootingSystems;
using UnityEngine;
using UpgradeSystem;

namespace Projectiles
{
    public class APCR : AP
    {
        public override ProjectileType Type => ProjectileType.APCR;

        private const int EnvironmentHitsLimit = 2;
        private int _environmentHits;
        
        public APCR(ProjectileProps props, UpgradeBroker upgradeBroker, object owner,
            Vector3 position, Vector3 direction) : base(props, upgradeBroker, owner, position, direction)
        {
            _baseNormalization = GlobalSettings.Normalizations[ProjectileType.APCR];
        }

        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(ArmorDamage, _owner);
                _environmentHits++;
            }
            else
            {
                IsInactive = true;
            }

            if (_environmentHits == EnvironmentHitsLimit)
            {
                IsInactive = true;
            }
        }
    }
}