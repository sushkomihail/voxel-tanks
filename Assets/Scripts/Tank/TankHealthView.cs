using UnityEngine;
using UnityEngine.UI;

namespace Tank
{
    [RequireComponent(typeof(Slider))]
    public class TankHealthView : MonoBehaviour
    {
        private Slider _slider;

        public void Init(int maxHealth)
        {
            _slider = GetComponent<Slider>();
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }
        
        public void UpdateSlider(int currentHealth)
        {
            _slider.value = currentHealth;
            // TODO: Make slider animation with DoTween
        }
    }
}
