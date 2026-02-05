using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected int _caliber = 100;
        [SerializeField] protected int _penetration = 100;
        [SerializeField] protected int[] _ricochetAngles;
        [SerializeField] private float _speed = 500f;
        [SerializeField] private float _mass = 25f;
        
        private Rigidbody _rigidbody;
        private bool _canMove;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.mass = _mass;
            _rigidbody.isKinematic = true;
        }

        private void Update()
        {
            if (!_canMove) return;
            
            Move();
        }
        
        [ContextMenu("Throw")]
        public void Throw()
        {
            _canMove = true;
            _rigidbody.isKinematic = false;
        }

        protected bool IsRicochet(float impactAngle)
        {
            float probability = Random.Range(0f, 1f);
            return probability < GetRicochetProbability(impactAngle);
        }

        protected abstract void HandleCollision(Collision other);
        
        private void Move()
        {
            _rigidbody.linearVelocity = transform.forward * _speed;
        }
        
        private float GetRicochetProbability(float impactAngle)
        {
            if (_ricochetAngles == null || _ricochetAngles.Length == 0) return 0;
            if (impactAngle <= _ricochetAngles[0]) return 0;
            if (impactAngle >= _ricochetAngles[^1]) return 1;

            for (int i = 0; i < _ricochetAngles.Length - 1; i++)
            {
                if (impactAngle >= _ricochetAngles[i] && impactAngle <= _ricochetAngles[i + 1])
                {
                    float k = (impactAngle - _ricochetAngles[i]) / (_ricochetAngles[i + 1] - _ricochetAngles[i]);
                    return (i + 1) / 2.0f * k;
                }
            }
            
            return 0;
        }

        private void OnCollisionEnter(Collision other)
        {
            _rigidbody.isKinematic = true;
            HandleCollision(other);
        }
    }
}