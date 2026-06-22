using UI;
using UnityEngine;
using VContainer;

namespace Tank.View
{
    public class PlayerView : TankView
    {
        private HealthBar _hudHealthBar;
        
        [Inject]
        public void Construct(HealthBar hudHealthBar)
        {
            _hudHealthBar = hudHealthBar;
        }

        public override void UpdateHealthVisuals(int currentHealth, int maxHealth)
        {
            base.UpdateHealthVisuals(currentHealth, maxHealth);
            
            float healthPercent = Mathf.Clamp01((float)currentHealth / maxHealth);
            _hudHealthBar.UpdateSlider(healthPercent);
            _hudHealthBar.UpdateHealthText(currentHealth, maxHealth);
        }
    }
}