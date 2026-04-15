using Tank.Data;

namespace Environment.Base
{
    public class RecapturingState : BaseState
    {
        public RecapturingState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankEntersBase(TankBattleData tankData)
        {
            base.OnTankEntersBase(tankData);
            
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

        public override void OnTankLeavesBase(TankBattleData tankData)
        {
            base.OnTankLeavesBase(tankData);

            if (tankData.Id == _baseModel.OwnerId)
            {
                _baseModel.SetOwnerId(_baseModel.PreviousOwnerId);
            }
            
            _baseModel.SetState(_baseModel.CapturedState);
        }
    }
}