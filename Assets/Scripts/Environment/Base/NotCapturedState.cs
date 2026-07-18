using Tank;

namespace Environment.Base
{
    public class NotCapturedState : BaseState
    {
        public NotCapturedState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankEntersBase(TankController tankController)
        {
            base.OnTankEntersBase(tankController);
            
            _baseModel.SetOwnerController(tankController);
            _baseModel.SetState(_baseModel.CapturingState);
        }

        public override void UpdateCaptureProgress()
        {
            if (_baseModel.CapturingProgress > 0f)
            {
                _baseModel.DecreaseCapturingProgress();
            }
        }
    }
}