using ArmorSystem;
using Settings;
using ShootingSystems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public abstract class Projectile
    {
        public virtual ProjectileType Type => ProjectileType.AP;
        
        public float Scale => _props.Scale;
        public Vector3 CurrentPosition { get; private set; }
        public Vector3 Velocity { get; private set; }
        public bool IsInactive { get; protected set; }

        protected readonly ProjectileProps _props;
        protected float _basePenetration;
        protected float _baseNormalization;
        
        private float _flightDistance;

        protected Projectile(ProjectileProps props, Vector3 position, Vector3 direction)
        {
            _props = props;
            _basePenetration = _props.Penetration;
            CurrentPosition = position;
            Velocity = direction * _props.Speed;
        }

        public void Update(float deltaTime)
        {
            if (IsInactive) return;

            Velocity += Physics.gravity * deltaTime;
            
            Vector3 moveDirection = Velocity * deltaTime;
            float moveDistance = moveDirection.magnitude;

            _flightDistance += moveDistance;
            if (_flightDistance > _props.MaxFlightDistance)
            {
                IsInactive = true;
                return;
            }

#if UNITY_EDITOR
            Debug.DrawRay(CurrentPosition, moveDirection, Color.red, 5f);
#endif

            if (Physics.Raycast(CurrentPosition, moveDirection, out RaycastHit hit, moveDistance, _props.HitMask.value))
            {
                if (hit.collider.TryGetComponent(out Armor armor))
                {
                    HandleArmorHit(armor, hit, moveDirection.normalized);
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
                
                moveDistance -= hit.distance;
                moveDirection = Velocity.normalized;
            }
            
            CurrentPosition += moveDirection * moveDistance;
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
                armor.TakeDamage(_props);
                return true;
            }
            
            return false;
        }
    }
}