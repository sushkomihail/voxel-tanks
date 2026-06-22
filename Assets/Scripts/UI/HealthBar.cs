using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillRect;
        [SerializeField] private TMP_Text _healthText;

        public void UpdateSlider(float value)
        {
            _fillRect.fillAmount = value;
            // TODO: Make slider animation with DoTween
        }

        public void UpdateHealthText(int currentHealth, int maxHealth)
        {
            _healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
