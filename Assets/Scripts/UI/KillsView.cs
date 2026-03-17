using TMPro;
using UnityEngine;

namespace UI
{
    public class KillsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _killsText;

        private void Awake()
        {
            _killsText.text = "0";
        }

        public void UpdateKillsText(int kills)
        {
            _killsText.text = kills.ToString();
        }
    }
}