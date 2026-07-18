using ShootingSystems;
using Tank;
using UnityEngine;
using UpgradeSystem;

namespace ArmorSystem
{
    public class Armor : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _thickness;

        private UpgradeBroker _upgradeBroker;
        private object _owner;
        
        public TankHealth TankHealth { get; private set; }
        
        public int Thickness
        {
            get
            {
                StatQuery query = new StatQuery(StatType.Armor, _thickness);
                _upgradeBroker.Query(_owner, query);
                return Mathf.RoundToInt(query.Value);
            }
        }
        
        public void Initialize(TankHealth tankHealth, UpgradeBroker upgradeBroker, object owner)
        {
            TankHealth = tankHealth;
            _upgradeBroker = upgradeBroker;
            _owner = owner;
        }
        
        public virtual void TakeDamage(int damage, object attacker)
        {
            TankHealth.TakeDamage(damage, attacker);
        }

        public float GetReducedThickness(Vector3 armorNormal, Vector3 hitDirection, float normalization, int caliber)
        {
            int thickness = Thickness;
            float angle = CalculateHitAngle(armorNormal, hitDirection, normalization, caliber);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            
            if (cos == 0) return thickness;
            return thickness / cos;
        }
        
        public float GetReducedThickness(float hitAngle)
        {
            int thickness = Thickness;
            float cos = Mathf.Cos(hitAngle * Mathf.Deg2Rad);
            
            if (cos == 0) return thickness;
            return thickness / cos;
        }

        public bool IsRicochet(Vector3 armorNormal, Vector3 hitDirection, float baseNormalization,
            float ricochetAngle, int caliber, out float hitAngle)
        {
            hitAngle = CalculateHitAngle(armorNormal, hitDirection, baseNormalization, caliber);

            if (caliber > Thickness * 3) return false;
            return hitAngle >= ricochetAngle;
        }

        private float CalculateHitAngle(Vector3 armorNormal, Vector3 hitDirection, float baseNormalization, int caliber)
        {
            int thickness = Thickness;
            float realNormalization = baseNormalization;
            
            if (caliber > thickness * 2)
            {
                realNormalization = baseNormalization * 1.4f * caliber / thickness;
            }
            
            return Mathf.Clamp(Vector3.Angle(-armorNormal, hitDirection) - realNormalization, 0, 90);
        }
    }
}