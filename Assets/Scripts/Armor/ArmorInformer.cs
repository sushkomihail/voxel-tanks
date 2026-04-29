using UnityEngine;

namespace Armor
{
    public class ArmorInformer
    {
        private Transform _lastTransform;
        private Transform _lastArmorTransform;
        private Armor _lastArmor;
        
        public float GetReducedThickness(Transform hitTransform, Vector3 hitNormal, Vector3 hitDirection)
        {
            if (!hitTransform) return -1;
            
            if (hitTransform == _lastArmorTransform)
            {
                if (_lastArmor) return _lastArmor.GetReducedThickness(hitNormal, hitDirection);
                return -1;
            }

            if (hitTransform == _lastTransform) return -1;

            if (hitTransform.TryGetComponent(out Armor armor))
            {
                _lastArmorTransform = hitTransform;
                _lastArmor = armor;
                return _lastArmor.GetReducedThickness(hitNormal, hitDirection);
            }
            
            _lastTransform = hitTransform;
            return -1;
        }
    }
}