using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Environment.Base
{
    public class BaseView : MonoBehaviour
    {
        [SerializeField] private Image _capturingProgressSlider;
        [SerializeField] private Color _playerCapturingColor = new(166, 224, 63, 255);
        [SerializeField] private Color _enemyCapturingColor = new(225, 45, 45, 255);
        [SerializeField] private TMP_Text _capturingProgressText;
        [SerializeField] private TMP_Text _baseNameText;

        private Color _currentCapturingColor;

        public void Initialize(char baseName)
        {
            _baseNameText.text = baseName.ToString();
            UpdateCapturingProgressSlider(0, false, false);
            UpdateCapturingProgressText(0);
        }

        public void UpdateCapturingProgressSlider(float capturingRate, bool isRecapturing, bool isPlayerInBase)
        {
            if (!isRecapturing)
            {
                if (isPlayerInBase)
                {
                    _currentCapturingColor = _playerCapturingColor;
                }
                else
                {
                    _currentCapturingColor = _enemyCapturingColor;
                }
            }
            
            _capturingProgressSlider.color = _currentCapturingColor;
            _capturingProgressSlider.fillAmount = capturingRate;
        }
        
        public void UpdateCapturingProgressText(float capturingRate)
        {
            _capturingProgressText.text = Mathf.FloorToInt(capturingRate * 100).ToString();
        }
    }
}