using Settings;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public class APCR : AP
    {
        public override ProjectileType Type => ProjectileType.APCR;

        private const int EnvironmentHitsLimit = 2;
        private int _environmentHits;
        
        public APCR(ProjectileProps props, Vector3 position, Vector3 direction) : base(props, position, direction)
        {
            _baseNormalization = GlobalSettings.Normalizations[ProjectileType.APCR];
        }

        protected override void HandleEnvironmentHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_props);
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