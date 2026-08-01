using Databases;
using Environment.Base;
using Tank;
using UI.Aims;
using UnityEngine;
using UnityEngine.InputSystem;
using UpgradeSystem;
using VContainer;
using PlayerInput = InputSystem.PlayerInput;

namespace UI
{
    public class UILoader : MonoBehaviour
    {
        [SerializeField] private AimsDatabase _aimsDatabase;
        [SerializeField] private ProjectilesSelector _projectilesSelector;
        [SerializeField] private UpgradesSelector _upgradesSelector;
        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private DrivingView _drivingView;
        [SerializeField] private Transform _baseViewsContainer;
        [SerializeField] private BaseView _baseViewPrefab;
        
        private GameInput _inputActions;
        private TankCamera _tankCamera;
        private UpgradeManager _upgradeManager;
        private TankController _playerTankController;

        [Inject]
        public void Construct(GameInput inputActions, TankCamera tankCamera, UpgradeManager upgradeManager)
        {
            _inputActions = inputActions;
            _tankCamera = tankCamera;
            _upgradeManager = upgradeManager;
        }

        public void Initialize(TankController playerController)
        {
            _inputActions.Global.SwitchCursor.started += OnCursorStarted;
            _inputActions.Global.SwitchCursor.canceled += OnCursorCanceled;
            
            _playerTankController = playerController;
            
            InitializeAim();
            
            InitializeProjectilesSelector();
            
            _upgradesSelector.Initialize(_upgradeManager, playerController);
            
            _drivingView.Initialize(_playerTankController.Chassis);
            
            CursorSwitch.DisableCursor();
        }

        private void OnDestroy()
        {
            _inputActions.Global.SwitchCursor.started -= OnCursorStarted;
            _inputActions.Global.SwitchCursor.canceled -= OnCursorCanceled;
            
            _playerTankController.Gun.ShootingSystem.OnCurrentProjectileTypeChanged -=
                _projectilesSelector.SetNextItemAsCurrent;
            
            _projectilesSelector.OnCurrentItemChanged -=
                _playerTankController.Gun.ShootingSystem.SetNextProjectileTypeAsCurrent;
            
            _projectilesSelector.OnNextItemChanged -= 
                _playerTankController.Gun.ShootingSystem.AddNextProjectileTypeToQueue;
        }

        private void OnCursorStarted(InputAction.CallbackContext _)
        {
            CursorSwitch.EnableCursor();
            _playerTankController.SetCombatActionsEnabled(false);
        }

        private void OnCursorCanceled(InputAction.CallbackContext _)
        {
            CursorSwitch.DisableCursor();
            _playerTankController.SetCombatActionsEnabled(true);
        }

        public BaseView InstantiateBaseView(int index)
        {
            BaseView view = Instantiate(_baseViewPrefab, _baseViewsContainer);
            view.Initialize((char)('A' + index));
            return view;
        }

        private void InitializeAim()
        {
            Aim aimPrefab = _aimsDatabase.GetAimByShootingSystem(_playerTankController.Gun.ShootingSystem);

            if (!aimPrefab) return;
            
            Aim aim = Instantiate(aimPrefab, transform);
            aim.Initialize(_playerTankController.Gun, _tankCamera.Camera);
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
    }
}