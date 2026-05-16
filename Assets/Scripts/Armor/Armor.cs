using Projectiles;
using ShootingSystems;
using Tank;
using UnityEngine;

namespace Armor
{
    public class Armor : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _thickness;
        
        public int Thickness => _thickness;
        
        private TankHealth _tankHealth;

        public void Initialize(TankHealth tankHealth)
        {
            _tankHealth = tankHealth;
        }
        
        public void TakeDamage(ProjectileProps props)
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

        public bool IsRicochet(Vector3 armorNormal, Vector3 hitDirection, float normalization, float ricochetAngle)
        {
            float angle = CalculateHitAngle(armorNormal, hitDirection, normalization);
            return angle >= ricochetAngle;
        }

        private static float CalculateHitAngle(Vector3 armorNormal, Vector3 hitDirection, float normalization)
        {
            return Mathf.Clamp(Vector3.Angle(-armorNormal, hitDirection) - normalization, 0, 90);
        }
    }
}