using Tank.Data;

namespace Environment.Base
{
    public class CapturingPauseState : BaseState
    {
        public CapturingPauseState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankLeavesBase(TankBattleData tankData)
        {
            base.OnTankLeavesBase(tankData);
            
            if (_baseModel.GetInsideBaseTanksCount() > 1) return;

            if (tankData.Id == _baseModel.OwnerId)
            {
                _baseModel.SetOwnerId(tankData.Id);
                _baseModel.SetState(_baseModel.RecapturingState);
            }
            else
            {
                _baseModel.SetState(_baseModel.CapturingState);
            }
        }
    }
}