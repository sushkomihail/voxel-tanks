using Environment;
using UnityEngine;

namespace ShootingSystems
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ShootingSystem _shootingSystem;
        
        public ProjectileType Type { get; private set; }
        public ProjectileProps Props { get; private set; }

        private void Update()
        {
            Rotate();
        }

        public void Init(ProjectileType type, ProjectileProps props, ShootingSystem shootingSystem)
        {
            _rigidbody = GetComponent<Rigidbody>();
            Type = type;
            Props = props;
            _shootingSystem = shootingSystem;
        }
        
        public void Launch(Transform pivot)
        {
            transform.position = pivot.position;
            transform.rotation = pivot.rotation;
            _rigidbody.linearVelocity = pivot.forward * Props.Speed;
        }

        private void Rotate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
            transform.rotation = targetRotation;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IDamageable damageableObject))
            {
                damageableObject.TakeDamage(Props.BaseDamage);
            }
            
            _shootingSystem.OnProjectileHit(this);
        }
    }
}