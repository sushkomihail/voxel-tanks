using Tank.Data;

namespace Environment.Base
{
    public class RecapturingPauseState : BaseState
    {
        public RecapturingPauseState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }
        
        public override void OnTankLeavesBase(TankBattleData tankData)
        {
            base.OnTankLeavesBase(tankData);
            
            if (_baseModel.GetInsideBaseTanksCount() > 1) return;

            if (_baseModel.TryGetFirstInsideBaseTankId(out string id))
            {
                _baseModel.SetOwnerId(id);
                _baseModel.SetState(_baseModel.RecapturingState);
            }
        }
    }
}