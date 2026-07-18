using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;
using UpgradeSystem;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public abstract class Projectile
    {
        protected readonly ProjectileProps _props;
        protected readonly object _owner;
        protected float _basePenetration;
        protected float _baseNormalization;
        
        private readonly UpgradeBroker _upgradeBroker;
        private float _flightDistance;
        
        public virtual ProjectileType Type => ProjectileType.AP;
        
        public float Scale => _props.Scale;
        public Vector3 CurrentPosition { get; private set; }
        public Vector3 Velocity { get; private set; }
        public bool IsInactive { get; protected set; }

        protected int ArmorDamage {
            get
            {
                StatQuery query = new StatQuery(StatType.Damage, _props.ArmorDamage);
                _upgradeBroker.Query(_owner, query);
                return (int)query.Value;
            }
        }

        protected int ModuleDamage
        {
            get
            {
                StatQuery query = new StatQuery(StatType.Damage, _props.ModuleDamage);
                _upgradeBroker.Query(_owner, query);
                return (int)query.Value;
            }
        }

        protected Projectile(ProjectileProps props, UpgradeBroker upgradeBroker, object owner,
            Vector3 position, Vector3 direction)
        {
            _props = props;
            _owner = owner;
            _upgradeBroker = upgradeBroker;
            
            _basePenetration = _props.Penetration;
            CurrentPosition = position;
            Velocity = direction * _props.Speed;
        }

        public void Update()
        {
            if (IsInactive) return;

            Velocity += Physics.gravity * Time.fixedDeltaTime;
            
            Vector3 moveDirection = Velocity * Time.fixedDeltaTime;
            Vector3 moveDirectionNormalized = moveDirection.normalized;
            float moveDistance = moveDirection.magnitude;

            if (_flightDistance + moveDistance > _props.MaxFlightDistance)
            {
                moveDistance = _props.MaxFlightDistance - _flightDistance;
            }

            if (Physics.Raycast(CurrentPosition, moveDirectionNormalized, out RaycastHit hit, moveDistance, _props.HitMask.value))
            {
                if (hit.collider.TryGetComponent(out Armor armor))
                {
                    HandleArmorHit(armor, hit, moveDirectionNormalized);
                }
                else
                {
                    HandleEnvironmentHit(hit);
                }

                if (IsInactive)
                {
                    CurrentPosition = hit.point;
                    return;
                }
                
                moveDirection = Velocity * Time.fixedDeltaTime;
                moveDirectionNormalized = moveDirection.normalized;
                moveDistance -= hit.distance;
            }
            
            CurrentPosition += moveDirectionNormalized * moveDistance;
        }

        protected abstract void HandleEnvironmentHit(RaycastHit hit);

        protected abstract void HandleArmorHit(Armor armor, RaycastHit hit, Vector3 hitDirection);

        protected float CalculateRealPenetration()
        {
            float penetrationRatio =
                1 + Random.Range(-GlobalSettings.PenetrationError, GlobalSettings.PenetrationError);
            return _basePenetration * penetrationRatio;
        }

        protected void OnRicochet(RaycastHit hit)
        {
            CurrentPosition = hit.point + hit.normal * 0.01f;
            Velocity = Vector3.Reflect(Velocity, hit.normal);
        }

        protected bool TryDealDamageToArmor(Armor armor, float reducedThickness, float realPenetration)
        {
            if (reducedThickness <= realPenetration)
            {
                armor.TakeDamage(ArmorDamage, _owner);
                return true;
            }
            
            return false;
        }
    }
}