using SaveSystem;
using SaveSystem.SavableStructures;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private TankSelectionPanel _tankSelectionPanel;
        [SerializeField] private Button _playButton;

        private void Awake()
        {
            _playButton.onClick.AddListener(Play);
        }
        
        private void Play()
        {
            int tankId = _tankSelectionPanel.GetSelectedTankId();
            
            if (tankId == -1) return;
            
            Saver<BattleData>.Save(new BattleData(tankId), nameof(BattleData));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
