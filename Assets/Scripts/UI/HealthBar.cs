using Tank;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillRect;
        private TankHealth _health;

        public void Initialize(TankHealth health)
        {
            _health = health;
            _health.OnHealthChanged += UpdateSlider;
            
            _fillRect.fillAmount = 1;
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= UpdateSlider;
        }

        private void UpdateSlider(int currentHealth, int maxHealth)
        {
            _fillRect.fillAmount = (float)currentHealth / maxHealth;
            // TODO: Make slider animation with DoTween
        }
    }
}
