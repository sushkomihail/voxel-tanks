using Tank.Data;

namespace Environment.Base
{
    public class CapturingState : BaseState
    {
        public CapturingState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankEntersBase(TankBattleData tankData)
        {
            base.OnTankEntersBase(tankData);
            
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

        public override void OnTankLeavesBase(TankBattleData tankData)
        {
            base.OnTankLeavesBase(tankData);
            
            _baseModel.SetState(_baseModel.NotCapturedState);
        }
    }
}