using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillRect;
        [SerializeField] private TMP_Text _currentHealthText;
        
        public void Initialize()
        {
            _fillRect.fillAmount = 1;
        }

        public void UpdateSlider(float value)
        {
            _fillRect.fillAmount = value;
            // TODO: Make slider animation with DoTween
        }

        public void UpdateCurrentHealthText(int currentHealth, int maxHealth)
        {
            _currentHealthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
