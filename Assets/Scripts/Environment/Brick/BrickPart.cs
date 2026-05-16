using Projectiles;
using ShootingSystems;
using UnityEngine;

namespace Environment.Brick
{
    [RequireComponent(typeof(MeshCollider))]
    public class BrickPart : MonoBehaviour, IDamageable
    {
        [SerializeField] private BrickPart _nextPart;
        [SerializeField] private bool _hideOnHit;

        private Brick _brick;

        public MeshCollider Collider { get; private set; }
        
        public void Init(Brick brick)
        {
            Collider = GetComponent<MeshCollider>();
            _brick = brick;
        }

        public void TakeDamage(ProjectileProps props)
        {
            Collider.enabled = false;
            _nextPart?.TakeDamage(props);
            
            if (_hideOnHit)
            {
                gameObject.SetActive(false);
            }
            
            _brick.OnPartDamaged(this);
        }
    }
}