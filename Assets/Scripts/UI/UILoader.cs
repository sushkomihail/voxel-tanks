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
        private PlayerController _playerController;

        [Inject]
        public void Construct(GameInput inputActions, TankCamera tankCamera, UpgradeManager upgradeManager)
        {
            _inputActions = inputActions;
            _tankCamera = tankCamera;
            _upgradeManager = upgradeManager;
        }

        public void Initialize(PlayerController playerController)
        {
            _inputActions.Global.SwitchCursor.started += OnCursorStarted;
            _inputActions.Global.SwitchCursor.canceled += OnCursorCanceled;
            
            _playerController = playerController;
            
            InitializeAim(playerController);
            
            InitializeProjectilesSelector();
            
            _upgradesSelector.Initialize(_upgradeManager, playerController);
            
            _drivingView.Initialize(_playerController.Chassis);
            
            CursorSwitch.DisableCursor();
        }

        private void OnDestroy()
        {
            _inputActions.Global.SwitchCursor.started -= OnCursorStarted;
            _inputActions.Global.SwitchCursor.canceled -= OnCursorCanceled;
            
            _playerController.Gun.ShootingSystem.OnCurrentProjectileTypeChanged -=
                _projectilesSelector.SetNextItemAsCurrent;
            
            _projectilesSelector.OnCurrentItemChanged -=
                _playerController.Gun.ShootingSystem.SetNextProjectileTypeAsCurrent;
            
            _projectilesSelector.OnNextItemChanged -= 
                _playerController.Gun.ShootingSystem.AddNextProjectileTypeToQueue;
        }

        private void OnCursorStarted(InputAction.CallbackContext _)
        {
            CursorSwitch.EnableCursor();
            _playerController.SetCombatActionsEnabled(false);
        }

        private void OnCursorCanceled(InputAction.CallbackContext _)
        {
            CursorSwitch.DisableCursor();
            _playerController.SetCombatActionsEnabled(true);
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
    }
}