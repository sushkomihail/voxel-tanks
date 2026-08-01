using InputSystem;

namespace Tank
{
    public abstract class TankConfigurator
    {
        protected readonly TankController _controller;
        
        protected TankConfigurator(TankController controller)
        {
            _controller = controller;
        }
        
        public abstract IInput GetInput();
        public abstract void Configure();
    }
}