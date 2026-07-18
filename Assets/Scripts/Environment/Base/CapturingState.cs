using Tank;
using Tank.Data;

namespace Environment.Base
{
    public class CapturingState : BaseState
    {
        public CapturingState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankEntersBase(TankController tankController)
        {
            base.OnTankEntersBase(tankController);
            
            _baseModel.SetState(_baseModel.CapturingPauseState);
        }

        public override void UpdateCaptureProgress()
        {
            _baseModel.IncreaseCapturingProgress();

            if (_baseModel.CapturingProgress == _baseModel.CaptureTime)
            {
                _baseModel.SetState(_baseModel.CapturedState);
            }
        }

        public override void OnTankLeavesBase(TankController tankController)
        {
            base.OnTankLeavesBase(tankController);
            
            _baseModel.SetState(_baseModel.NotCapturedState);
        }
    }
}