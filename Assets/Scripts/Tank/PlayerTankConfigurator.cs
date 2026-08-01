using EquipmentSystem;
using InputSystem;

namespace Tank
{
    public class PlayerTankConfigurator : TankConfigurator
    {
        private readonly GameInput _inputActions;
        private readonly TankCamera _tankCamera;
        private readonly EquipmentPresenter _equipmentPresenter;
        
        public PlayerTankConfigurator(
            TankController controller,
            GameInput inputActions,
            TankCamera tankCamera,
            EquipmentPresenter equipmentPresenter
            ) : base(controller)
        {
            _inputActions = inputActions;
            _tankCamera = tankCamera;
            _equipmentPresenter = equipmentPresenter;
        }

        public override IInput GetInput()
        {
            return new PlayerInput(_inputActions, _tankCamera);
        }

        public override void Configure()
        {
            _equipmentPresenter.Initialize(_controller.Equipment);
            _controller.View.DisableOverTankHealthBar();

            _controller.TankCameraFollowing += () =>
            {
                _tankCamera.FollowTarget(_controller.CameraTarget, _controller.CameraFollowingOffset);
            };
            
            _controller.Health.OnHealthChanged += _controller.View.UpdateOverTankHealthBar;
            _controller.Health.OnHealthChanged += _controller.View.UpdateHudHealthBar;
        }
    }
}