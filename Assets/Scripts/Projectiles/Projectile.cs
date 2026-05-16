using Settings;
using ShootingSystems;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public ProjectileType Type { get; protected set; }

        protected ShootingSystem _shootingSystem;
        protected ProjectileProps _props;
        protected float _normalization;
        
        private Rigidbody _rigidbody;
        private bool _hasHit;
        
        private void Update()
        {
            Rotate();
        }

        public virtual void Initialize(ProjectileProps props, ShootingSystem shootingSystem)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _props = props;
            _shootingSystem = shootingSystem;
        }
        
        public void Launch(Transform pivot)
        {
            _hasHit = false;
            
            transform.position = pivot.position;
            transform.rotation = pivot.rotation;
            
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.linearVelocity = pivot.forward * _props.Speed;
        }

        private void Rotate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
            transform.rotation = targetRotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_hasHit) return;
            
            _hasHit = true;
            ContactPoint contact = collision.contacts[0];
            
            if (!contact.otherCollider.TryGetComponent(out IDamageable damageable)) return;
            
            if (damageable is Armor.Armor armor)
            {
                float reducedThickness = armor.GetReducedThickness(contact.normal, transform.forward, _normalization);
                float penetrationRatio =
                    1 + Random.Range(-GlobalSettings.PenetrationError, GlobalSettings.PenetrationError);
                float realPenetration = _props.Penetration * penetrationRatio;

                if (reducedThickness > realPenetration)
                {
                    _shootingSystem.OnProjectileHit(this);
                    return;
                }
            }
            
            damageable.TakeDamage(_props);
            _shootingSystem.OnProjectileHit(this);
        }
    }
}