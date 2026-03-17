using UI;
using UnityEngine;

namespace Managers
{
    public class KillsManager : MonoBehaviour
    {
        [SerializeField] private KillsView _view;
        
        public static KillsManager Instance { get; private set; }

        private int _counter;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public void IncreaseCounter()
        {
            _counter++;
            _view.UpdateKillsText(_counter);
        }
    }
}