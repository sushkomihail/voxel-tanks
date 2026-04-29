using Armor;
using Tank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Aims
{
    public abstract class Aim : MonoBehaviour
    {
        [SerializeField] protected Image _reloadIndicator;
        [SerializeField] private Image _gunAimImage;
        [SerializeField] private Image _penetrationIndicator;
        [SerializeField] private Color _penetrationColor = new(166, 224, 63);
        [SerializeField] private Color _mootPenetrationColor = new(251, 142, 47);
        [SerializeField] private Color _nonPenetrationColor = new(225, 45, 45);
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
            float reducedThickness = _armorInformer.GetReducedThickness(hitTransform, hitNormal, hitDirection);
            
            UpdateReloadIndicator();
            UpdateGunAim(aimPoint);
            UpdatePenetrationIndicator(reducedThickness, _gun.ShootingSystem.GetProjectilePenetration());
            UpdateArmorText(reducedThickness);
        }

        protected abstract void UpdateReloadIndicator();

        private void UpdateGunAim(Vector3 aimPoint)
        {
            if (!_gun) return;
            
            Vector3 screenPoint = _camera.WorldToScreenPoint(aimPoint);
            _gunAimImage.rectTransform.position = screenPoint;
        }

        private void UpdatePenetrationIndicator(float reducedThickness, float penetration)
        {
            if (penetration == -1 || reducedThickness == -1)
            {
                _penetrationIndicator.color = Color.white;
                return;
            }

            if (reducedThickness > penetration)
            {
                _penetrationIndicator.color = _nonPenetrationColor;
                return;
            }

            _penetrationIndicator.color = _penetrationColor;
        }

        private void UpdateArmorText(float reducedThickness)
        {
            if (reducedThickness == -1)
            {
                _armorText.text = "";
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