using UnityEngine;

namespace Armor
{
    public class ArmorInformer
    {
        private Transform _lastTransform;
        private Transform _lastArmorTransform;
        private Armor _lastArmor;
        
        public (ArmorInfoCode, float) GetReducedThickness(Transform hitTransform, Vector3 hitNormal, Vector3 hitDirection,
            float normalization, float ricochetAngle)
        {
            if (!hitTransform) return (ArmorInfoCode.NotFound, 0);
            
            if (hitTransform == _lastArmorTransform)
            {
                if (_lastArmor)
                    return CheckForRicochetAndSetReducedThickness(hitNormal, hitDirection, normalization, ricochetAngle);
                return (ArmorInfoCode.NotFound, 0);
            }

            if (hitTransform == _lastTransform) return (ArmorInfoCode.NotFound, 0);

            if (hitTransform.TryGetComponent(out Armor armor))
            {
                _lastArmorTransform = hitTransform;
                _lastArmor = armor;
                return CheckForRicochetAndSetReducedThickness(hitNormal, hitDirection, normalization, ricochetAngle);
            }
            
            _lastTransform = hitTransform;
            return (ArmorInfoCode.NotFound, 0);
        }

        private (ArmorInfoCode, float) CheckForRicochetAndSetReducedThickness(Vector3 hitNormal, Vector3 hitDirection,
            float normalization, float ricochetAngle)
        {
            if (_lastArmor.IsRicochet(hitNormal, hitDirection, normalization, ricochetAngle) && ricochetAngle != -1)
            {
                return (ArmorInfoCode.Ricochet, 0);
            }
            
            return (ArmorInfoCode.Ok, _lastArmor.GetReducedThickness(hitNormal, hitDirection, normalization));
        }
    }
}