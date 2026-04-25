using System.Collections.Generic;
using Databases;
using Environment.Base;
using Tank;
using UI.Aims;
using UnityEngine;

namespace UI
{
    public class UILoader : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private AimsDatabase _aimsDatabase;
        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private HealthBar _npcHealthBarPrefab;
        [SerializeField] private Transform _baseViewsContainer;
        [SerializeField] private BaseView _baseViewPrefab;
        
        private HealthBar _bar;

        public void Initialize(PlayerTankController playerTankController, List<AITankController> npcTanks, TankCamera camera)
        {
            InitializeAim(playerTankController, camera);
            InitializeHealthBars(playerTankController, npcTanks);
            LockCursor();
        }

        public BaseView InstantiateBaseView(int index)
        {
            BaseView view = Instantiate(_baseViewPrefab, _baseViewsContainer);
            view.Initialize((char)('A' + index));
            return view;
        }

        private void InitializeAim(PlayerTankController playerTankController, TankCamera camera)
        {
            Aim aimPrefab = _aimsDatabase.GetAimByShootingSystem(playerTankController.Gun.ShootingSystem);

            if (!aimPrefab) return;
            
            Aim aim = Instantiate(aimPrefab, _canvas.transform);
            aim.Initialize(playerTankController.Gun, camera.Camera);
        }

        private void InitializeHealthBars(PlayerTankController playerTankController, List<AITankController> npcTanks)
        {
            // _playerHealthBar.Initialize(playerTankController.Health);
        }
        
        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}