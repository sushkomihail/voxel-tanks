using UnityEngine;

namespace ShootingSystems
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public ProjectileType Type { get; private set; }
        
        private Rigidbody _rigidbody;
        private ProjectileProps _props;
        private ShootingSystem _shootingSystem;
        private bool _hasHit;
        
        private void Update()
        {
            Rotate();
        }

        public void Initialize(ProjectileType type, ProjectileProps props, ShootingSystem shootingSystem)
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            Type = type;
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

        private void OnCollisionEnter(Collision other)
        {
            if (_hasHit) return;
            
            _hasHit = true;
            ContactPoint contact = other.contacts[0];
            
            if (!contact.otherCollider.TryGetComponent(out IDamageable damageable)) return;
            
            if (damageable is Armor.Armor armor)
            {
                float reducedThickness = armor.GetReducedThickness(contact.normal, transform.forward);

                if (reducedThickness > _props.Penetration)
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