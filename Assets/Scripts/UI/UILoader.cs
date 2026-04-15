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

        public void Initialize(PlayerTank playerTank, List<AITank> npcTanks, TankCamera camera)
        {
            InitializeAim(playerTank, camera);
            InitializeHealthBars(playerTank, npcTanks);
            LockCursor();
        }

        public BaseView InstantiateBaseView(int index)
        {
            BaseView view = Instantiate(_baseViewPrefab, _baseViewsContainer);
            view.Initialize((char)('A' + index));
            return view;
        }

        private void InitializeAim(PlayerTank playerTank, TankCamera camera)
        {
            Aim aimPrefab = _aimsDatabase.GetAimByShootingSystem(playerTank.Gun.ShootingSystem);

            if (!aimPrefab) return;
            
            Aim aim = Instantiate(aimPrefab, _canvas.transform);
            aim.Initialize(playerTank.Gun, camera.Camera);
        }

        private void InitializeHealthBars(PlayerTank playerTank, List<AITank> npcTanks)
        {
            _playerHealthBar.Initialize(playerTank.Health);

            foreach (AITank npcTank in npcTanks)
            {
                HealthBar healthBar = Instantiate(_npcHealthBarPrefab, npcTank.View.Canvas.transform);
                healthBar.Initialize(npcTank.Health);
            }
        }
        
        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}