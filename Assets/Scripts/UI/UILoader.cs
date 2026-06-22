using Databases;
using Environment.Base;
using InputSystem;
using Tank;
using UI.Aims;
using UnityEngine;
using VContainer;

namespace UI
{
    public class UILoader : MonoBehaviour
    {
        [SerializeField] private AimsDatabase _aimsDatabase;
        [SerializeField] private ProjectilesSelector _projectilesSelector;
        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private TankChassisView _tankChassisView;
        [SerializeField] private Transform _baseViewsContainer;
        [SerializeField] private BaseView _baseViewPrefab;
        
        private TankCamera _tankCamera;
        private PlayerController _playerController;

        [Inject]
        public void Construct(TankCamera tankCamera)
        {
            _tankCamera = tankCamera;
        }

        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
            
            InitializeAim(playerController);
            InitializeProjectilesSelector();
            _tankChassisView.Initialize(_playerController.Chassis);
            LockCursor();
        }

        private void OnDestroy()
        {
            _playerController.Gun.ShootingSystem.OnCurrentProjectileTypeChanged -=
                _projectilesSelector.SetNextItemAsCurrent;
            
            _projectilesSelector.OnCurrentItemChanged -=
                _playerController.Gun.ShootingSystem.SetNextProjectileTypeAsCurrent;
            
            _projectilesSelector.OnNextItemChanged -= 
                _playerController.Gun.ShootingSystem.AddNextProjectileTypeToQueue;
        }

        public BaseView InstantiateBaseView(int index)
        {
            BaseView view = Instantiate(_baseViewPrefab, _baseViewsContainer);
            view.Initialize((char)('A' + index));
            return view;
        }

        private void InitializeAim(PlayerController playerController)
        {
            Aim aimPrefab = _aimsDatabase.GetAimByShootingSystem(playerController.Gun.ShootingSystem);

            if (!aimPrefab) return;
            
            Aim aim = Instantiate(aimPrefab, transform);
            aim.Initialize(playerController.Gun, _tankCamera.Camera);
        }

        private void InitializeProjectilesSelector()
        {
            var projectileTypes = _playerController.Gun.ShootingSystem.ProjectileTypes;
            _projectilesSelector.Initialize(projectileTypes, _playerController.Input as PlayerInput);

            _playerController.Gun.ShootingSystem.OnCurrentProjectileTypeChanged +=
                _projectilesSelector.SetNextItemAsCurrent;

            _projectilesSelector.OnCurrentItemChanged +=
                _playerController.Gun.ShootingSystem.SetNextProjectileTypeAsCurrent;
            
            _projectilesSelector.OnNextItemChanged +=
                _playerController.Gun.ShootingSystem.AddNextProjectileTypeToQueue;
        }
        
        private static void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}