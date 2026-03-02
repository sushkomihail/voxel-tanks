using ShootingSystems;
using Tank;
using UnityEngine;

namespace Armor
{
    public class Armor : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _thickness;
        
        private TankHealth _tankHealth;

        public void Init(TankHealth tankHealth)
        {
            _tankHealth = tankHealth;
        }
        
        public void TakeDamage(ProjectileProps props)
        {
            Debug.Log("damaged");
            int damage = props.ArmorDamage;
            float penetrationRate = damage / _thickness;

            if (penetrationRate < 1)
            {
                damage = (int)(damage * penetrationRate);
            }
            
            _tankHealth.OnArmorDamaged(damage);
        }
    }
}