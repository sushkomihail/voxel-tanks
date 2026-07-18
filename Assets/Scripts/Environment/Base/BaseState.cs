using Tank;

namespace Environment.Base
{
    public abstract class BaseState
    {
        protected readonly BaseModel _baseModel;

        protected BaseState(BaseModel baseModelReference)
        {
            _baseModel = baseModelReference;
        }
        
        public virtual void Enter() {}
        
        public virtual void OnTankEntersBase(TankController tankController)
        {
            _baseModel.SetIsTankInsideBase(tankController, true);
        }
        
        public virtual void UpdateCaptureProgress() {}

        public virtual void OnTankLeavesBase(TankController tankController)
        {
            _baseModel.SetIsTankInsideBase(tankController, false);
        }
    }
}