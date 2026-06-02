using UnityEngine;

namespace ArmorSystem
{
    public class ArmorInformer
    {
        private Transform _lastTransform;
        private Transform _lastArmorTransform;
        private Armor _lastArmor;
        
        public (ArmorInfo, float) GetReducedThickness(Transform hitTransform, Vector3 hitNormal, Vector3 hitDirection,
            float normalization, float ricochetAngle)
        {
            if (!hitTransform) return (ArmorInfo.NotFound, 0);
            
            if (hitTransform == _lastArmorTransform)
            {
                if (_lastArmor)
                    return CheckForRicochetAndSetReducedThickness(hitNormal, hitDirection, normalization, ricochetAngle);
                return (ArmorInfo.NotFound, 0);
            }

            if (hitTransform == _lastTransform) return (ArmorInfo.NotFound, 0);

            if (hitTransform.TryGetComponent(out Armor armor))
            {
                _lastArmorTransform = hitTransform;
                _lastArmor = armor;
                return CheckForRicochetAndSetReducedThickness(hitNormal, hitDirection, normalization, ricochetAngle);
            }
            
            _lastTransform = hitTransform;
            return (ArmorInfo.NotFound, 0);
        }

        private (ArmorInfo, float) CheckForRicochetAndSetReducedThickness(Vector3 hitNormal, Vector3 hitDirection,
            float normalization, float ricochetAngle)
        {
            if (Armor.IsRicochet(hitNormal, hitDirection, normalization, ricochetAngle, out float hitAngle)
                && ricochetAngle != -1)
            {
                return (ArmorInfo.Ricochet, 0);
            }
            
            return (ArmorInfo.Thickness, _lastArmor.GetReducedThickness(hitAngle));
        }
    }
}