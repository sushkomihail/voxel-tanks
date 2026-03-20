using Tank;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Aims
{
    public abstract class Aim : MonoBehaviour
    {
        [SerializeField] protected Image _reloadIndicator;
        [SerializeField] protected Image _gunAimPointer;

        protected TankGun _gun;
        
        private Camera _camera;

        private void Update()
        {
            UpdateReloadIndicator();
            UpdateGunAimPointer();
        }

        public virtual void Initialize(TankGun gun, Camera camera)
        {
            _gun = gun;
            _camera = camera;
            
            if (_reloadIndicator)
            {
                _reloadIndicator.fillAmount = 0;   
            }
        }

        protected abstract void UpdateReloadIndicator();

        private void UpdateGunAimPointer()
        {
            if (!_gun) return;
            
            Vector3 aimPoint = _gun.GetAimPoint();
            Vector3 screenPoint = _camera.WorldToScreenPoint(aimPoint);
            _gunAimPointer.rectTransform.position = screenPoint;
        }
    }
}