using Projectiles;
using ShootingSystems;
using Tank;
using UnityEngine;

namespace ArmorSystem
{
    public class Armor : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _thickness;
        [SerializeField] private bool _isScreen;
        
        public int Thickness => _thickness;
        public bool IsScreen => _isScreen;
        
        private TankHealth _tankHealth;

        public void Initialize(TankHealth tankHealth)
        {
            _tankHealth = tankHealth;
        }
        
        public virtual void TakeDamage(ProjectileProps props)
        {
            _tankHealth.OnArmorDamaged(props.ArmorDamage);
        }

        public float GetReducedThickness(Vector3 armorNormal, Vector3 hitDirection, float normalization)
        {
            float angle = CalculateHitAngle(armorNormal, hitDirection, normalization);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            
            if (cos == 0) return _thickness;
            return _thickness / cos;
        }
        
        public float GetReducedThickness(float hitAngle)
        {
            float cos = Mathf.Cos(hitAngle * Mathf.Deg2Rad);
            
            if (cos == 0) return _thickness;
            return _thickness / cos;
        }

        public static bool IsRicochet(Vector3 armorNormal, Vector3 hitDirection, float normalization,
            float ricochetAngle, out float hitAngle)
        {
            hitAngle = CalculateHitAngle(armorNormal, hitDirection, normalization);
            return hitAngle >= ricochetAngle;
        }

        private static float CalculateHitAngle(Vector3 armorNormal, Vector3 hitDirection, float normalization)
        {
            return Mathf.Clamp(Vector3.Angle(-armorNormal, hitDirection) - normalization, 0, 90);
        }
    }
}