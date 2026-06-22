using Projectiles;
using ShootingSystems;
using Tank;
using UnityEngine;

namespace ArmorSystem
{
    public sealed class Armor : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _thickness;
        [SerializeField] private bool _isScreen;
        
        public TankHealth TankHealth { get; private set; }
        public int Thickness => _thickness;
        public bool IsScreen => _isScreen;
        

        public void Initialize(TankHealth tankHealth)
        {
            TankHealth = tankHealth;
        }
        
        public void TakeDamage(ProjectileProps props)
        {
            TankHealth.OnArmorDamaged(props.ArmorDamage);
        }

        public float GetReducedThickness(Vector3 armorNormal, Vector3 hitDirection, float normalization, int caliber)
        {
            float angle = CalculateHitAngle(armorNormal, hitDirection, normalization, caliber);
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

        public bool IsRicochet(Vector3 armorNormal, Vector3 hitDirection, float normalization,
            float ricochetAngle, int caliber, out float hitAngle)
        {
            hitAngle = CalculateHitAngle(armorNormal, hitDirection, normalization, caliber);

            if (caliber > _thickness * 3) return false;
            return hitAngle >= ricochetAngle;
        }

        private float CalculateHitAngle(Vector3 armorNormal, Vector3 hitDirection, float normalization, int caliber)
        {
            float realNormalization = normalization;
            
            if (caliber > _thickness * 2)
            {
                realNormalization = normalization * 1.4f * caliber / _thickness;
            }
            
            return Mathf.Clamp(Vector3.Angle(-armorNormal, hitDirection) - realNormalization, 0, 90);
        }
    }
}