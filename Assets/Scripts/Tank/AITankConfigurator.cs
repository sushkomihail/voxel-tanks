using InputSystem;

namespace Tank
{
    public class AITankConfigurator : TankConfigurator
    {
        public AITankConfigurator(TankController controller) : base(controller)
        {
        }

        public override IInput GetInput()
        {
            AIInput input = _controller.GetComponent<AIInput>();
            input.Initialize();
            return input;
        }

        public override void Configure()
        {
            _controller.Health.OnHealthChanged += _controller.View.UpdateOverTankHealthBar;
        }
    }
}