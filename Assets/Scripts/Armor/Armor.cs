using ShootingSystems;
using Tank;
using UnityEngine;

namespace Armor
{
    public class Armor : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _thickness;
        
        private TankHealth _tankHealth;

        public void Init(TankHealth tankHealth)
        {
            _tankHealth = tankHealth;
        }
        
        public void TakeDamage(ProjectileProps props)
        {
            _tankHealth.OnArmorDamaged(props.ArmorDamage);
        }

        public float GetReducedThickness(Vector3 armorNormal, Vector3 hitDirection)
        {
            float angle = Vector3.Angle(-armorNormal, hitDirection);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            
            if (cos == 0) return _thickness;
            return _thickness / cos;
        }
    }
}