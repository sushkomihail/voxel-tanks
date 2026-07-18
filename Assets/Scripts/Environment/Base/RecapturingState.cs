using Tank;

namespace Environment.Base
{
    public class RecapturingState : BaseState
    {
        public RecapturingState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankEntersBase(TankController tankController)
        {
            base.OnTankEntersBase(tankController);
            
            _baseModel.SetState(_baseModel.RecapturingPauseState);
        }

        public override void UpdateCaptureProgress()
        {
            if (_baseModel.CapturingProgress > 0)
            {
                _baseModel.DecreaseCapturingProgress();
            }
            else
            {
                _baseModel.SetState(_baseModel.CapturingState);
            }
        }

        public override void OnTankLeavesBase(TankController tankController)
        {
            base.OnTankLeavesBase(tankController);

            if (tankController == _baseModel.OwnerController)
            {
                _baseModel.SetOwnerController(_baseModel.PreviousOwnerController);
            }
            
            _baseModel.SetState(_baseModel.CapturedState);
        }
    }
}