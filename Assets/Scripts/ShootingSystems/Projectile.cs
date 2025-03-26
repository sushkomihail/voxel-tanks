using UnityEngine;

namespace ShootingSystems
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ProjectileData _data;
        private ShootingSystem _shootingSystem;
        
        private void Update()
        {
            Rotate();
        }
        
        public void Initialize(ProjectileData data, ShootingSystem shootingSystem)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _data = data;
            _shootingSystem = shootingSystem;
        }
        
        public void Launch(Transform pivot)
        {
            transform.position = pivot.position;
            transform.rotation = pivot.rotation;
            _rigidbody.linearVelocity = transform.forward * _data.FlightSpeed;
        }

        private void Rotate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
            transform.rotation = targetRotation;
        }

        private void OnCollisionEnter(Collision other)
        {
            _shootingSystem.OnProjectileHit(this);
        }
    }
}