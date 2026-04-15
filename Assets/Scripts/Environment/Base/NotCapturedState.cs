using Tank.Data;

namespace Environment.Base
{
    public class NotCapturedState : BaseState
    {
        public NotCapturedState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankEntersBase(TankBattleData tankData)
        {
            base.OnTankEntersBase(tankData);
            
            _baseModel.SetOwnerId(tankData.Id);
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