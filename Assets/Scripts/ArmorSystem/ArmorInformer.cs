using UnityEngine;

namespace ArmorSystem
{
    public class ArmorInformer
    {
        private Transform _lastTransform;
        private Transform _lastArmorTransform;
        private Armor _lastArmor;
        
        public (ArmorInfo, float) GetReducedThickness(Transform hitTransform, Vector3 hitNormal, Vector3 hitDirection,
            float normalization, float ricochetAngle, int caliber)
        {
            if (!hitTransform) return (ArmorInfo.NotFound, 0);
            
            if (hitTransform == _lastArmorTransform)
            {
                if (_lastArmor)
                    return GetReducedThickness(hitNormal, hitDirection, normalization, ricochetAngle, caliber);
                return (ArmorInfo.NotFound, 0);
            }

            if (hitTransform == _lastTransform) return (ArmorInfo.NotFound, 0);

            if (hitTransform.TryGetComponent(out Armor armor))
            {
                _lastArmorTransform = hitTransform;
                _lastArmor = armor;
                return GetReducedThickness(hitNormal, hitDirection, normalization, ricochetAngle, caliber);
            }
            
            _lastTransform = hitTransform;
            return (ArmorInfo.NotFound, 0);
        }

        private (ArmorInfo, float) GetReducedThickness(Vector3 hitNormal, Vector3 hitDirection,
            float normalization, float ricochetAngle, int caliber)
        {
            if (_lastArmor.IsRicochet(hitNormal, hitDirection, normalization, ricochetAngle, caliber, out float hitAngle)
                && ricochetAngle != -1)
            {
                return (ArmorInfo.Ricochet, 0);
            }
            
            return (ArmorInfo.Thickness, _lastArmor.GetReducedThickness(hitAngle));
        }
    }
}