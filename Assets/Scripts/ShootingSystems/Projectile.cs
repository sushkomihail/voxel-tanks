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
        

        private void Update()
        {
            Rotate();
        }

        public void Init(ProjectileType type, ProjectileProps props, ShootingSystem shootingSystem)
        {
            _rigidbody = GetComponent<Rigidbody>();
            Type = type;
            _props = props;
            _shootingSystem = shootingSystem;
        }
        
        public void Launch(Transform pivot)
        {
            transform.position = pivot.position;
            transform.rotation = pivot.rotation;
            _rigidbody.linearVelocity = pivot.forward * _props.Speed;
        }

        private void Rotate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
            transform.rotation = targetRotation;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.contacts[0].otherCollider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_props);
            }
            
            _shootingSystem.OnProjectileHit(this);
        }
    }
}