using Armor;
using Settings;
using Tank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Aims
{
    public abstract class Aim : MonoBehaviour
    {
        [SerializeField] protected Image _reloadIndicator;
        [SerializeField] private RectTransform _gunAim;
        [SerializeField] private Image _penetrationIndicator;
        [SerializeField] private Color _penetrationColor;
        [SerializeField] private Color _mootPenetrationColor;
        [SerializeField] private Color _nonPenetrationColor;
        [SerializeField] private TMP_Text _armorText;

        protected TankGun _gun;
        
        private Camera _camera;
        private ArmorInformer _armorInformer;

        public virtual void Initialize(TankGun gun, Camera camera)
        {
            _gun = gun;
            _camera = camera;
            _armorInformer = new ArmorInformer();
            
            if (_reloadIndicator)
            {
                _reloadIndicator.fillAmount = 0;
            }
        }
        
        private void Update()
        {
            Vector3 aimPoint = 
                _gun.PredictHitPoint(out Transform hitTransform, out Vector3 hitNormal, out Vector3 hitDirection);
            float normalization = _gun.ShootingSystem.GetProjectileNormalization();
            float ricochetAngle = _gun.ShootingSystem.GetProjectileRicochetAngle();
            (ArmorInfoCode code, float reducedThickness) = 
                _armorInformer.GetReducedThickness(hitTransform, hitNormal, hitDirection, normalization, ricochetAngle);
            
            UpdateReloadIndicator();
            UpdateGunAim(aimPoint);
            UpdatePenetrationIndicator(code, reducedThickness);
            UpdateArmorText(code, reducedThickness);
        }

        protected abstract void UpdateReloadIndicator();

        private void UpdateGunAim(Vector3 aimPoint)
        {
            if (!_gun) return;
            
            Vector3 screenPoint = _camera.WorldToScreenPoint(aimPoint);
            _gunAim.position = screenPoint;
        }

        private void UpdatePenetrationIndicator(ArmorInfoCode code, float reducedThickness)
        {
            float penetration = _gun.ShootingSystem.GetProjectilePenetration();
            
            if (penetration == -1 || code == ArmorInfoCode.NotFound)
            {
                _penetrationIndicator.color = Color.white;
                return;
            }

            if (code == ArmorInfoCode.Ricochet)
            {
                _penetrationIndicator.color = _nonPenetrationColor;
                return;
            }

            float minPenetration = penetration * (1 - GlobalSettings.PenetrationError);
            float maxPenetration = penetration * (1 + GlobalSettings.PenetrationError);

            if (reducedThickness < minPenetration)
            {
                _penetrationIndicator.color = _penetrationColor;
            }
            else if (reducedThickness > maxPenetration)
            {
                _penetrationIndicator.color = _nonPenetrationColor;
            }
            else
            {
                _penetrationIndicator.color = _mootPenetrationColor;   
            }
        }

        private void UpdateArmorText(ArmorInfoCode code, float reducedThickness)
        {
            if (code == ArmorInfoCode.NotFound)
            {
                _armorText.text = "";
                return;
            }

            if (code == ArmorInfoCode.Ricochet)
            {
                // TODO: Add localization
                _armorText.text = "Рикошет";
                return;
            }

            if (reducedThickness > 999)
            {
                _armorText.text = "\u221E"; // infinity char
                return;
            }
            
            _armorText.text = $"~{reducedThickness:F1}";
        }
    }
}