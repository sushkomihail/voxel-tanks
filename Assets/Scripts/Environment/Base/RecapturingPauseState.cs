using Tank;

namespace Environment.Base
{
    public class RecapturingPauseState : BaseState
    {
        public RecapturingPauseState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }
        
        public override void OnTankLeavesBase(TankController tankController)
        {
            base.OnTankLeavesBase(tankController);
            
            if (_baseModel.GetInsideBaseTanksCount() > 1) return;

            if (_baseModel.TryGetFirstInsideBaseTankController(out TankController newOwnerController))
            {
                _baseModel.SetOwnerController(newOwnerController);
                _baseModel.SetState(_baseModel.RecapturingState);
            }
        }
    }
}