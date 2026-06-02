using Databases;
using Environment.Base;
using InputSystem;
using Tank;
using UI.Aims;
using UnityEngine;

namespace UI
{
    public class UILoader : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private AimsDatabase _aimsDatabase;
        [SerializeField] private ProjectilesSelector _projectilesSelector;
        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private Transform _baseViewsContainer;
        [SerializeField] private BaseView _baseViewPrefab;
        
        private PlayerTankController _playerTankController;

        public void Initialize(PlayerTankController playerTankController, TankCamera camera)
        {
            _playerTankController = playerTankController;
            
            InitializeAim(playerTankController, camera);
            InitializeProjectilesSelector();
            InitializePlayerHealthBar(playerTankController);
            LockCursor();
        }

        private void OnDestroy()
        {
            _playerTankController.Gun.ShootingSystem.OnCurrentProjectileTypeChanged -=
                _projectilesSelector.SetNextItemAsCurrent;
            
            _projectilesSelector.OnCurrentItemChanged -=
                _playerTankController.Gun.ShootingSystem.SetNextProjectileTypeAsCurrent;
            
            _projectilesSelector.OnNextItemChanged -= 
                _playerTankController.Gun.ShootingSystem.AddNextProjectileTypeToQueue;
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

        private void InitializeProjectilesSelector()
        {
            var projectileTypes = _playerTankController.Gun.ShootingSystem.ProjectileTypes;
            _projectilesSelector.Initialize(projectileTypes, _playerTankController.Input as PlayerInput);

            _playerTankController.Gun.ShootingSystem.OnCurrentProjectileTypeChanged +=
                _projectilesSelector.SetNextItemAsCurrent;

            _projectilesSelector.OnCurrentItemChanged +=
                _playerTankController.Gun.ShootingSystem.SetNextProjectileTypeAsCurrent;
            
            _projectilesSelector.OnNextItemChanged +=
                _playerTankController.Gun.ShootingSystem.AddNextProjectileTypeToQueue;
        }

        private void InitializePlayerHealthBar(PlayerTankController playerTankController)
        {
            _playerHealthBar.Initialize();
            playerTankController.View.SetPlayerHealthBar(_playerHealthBar);
        }
        
        private static void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}