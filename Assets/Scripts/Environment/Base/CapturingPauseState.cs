using Tank;

namespace Environment.Base
{
    public class CapturingPauseState : BaseState
    {
        public CapturingPauseState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void OnTankLeavesBase(TankController tankController)
        {
            base.OnTankLeavesBase(tankController);
            
            if (_baseModel.GetInsideBaseTanksCount() > 1) return;

            if (tankController == _baseModel.OwnerController)
            {
                _baseModel.SetOwnerController(tankController);
                _baseModel.SetState(_baseModel.RecapturingState);
            }
            else
            {
                _baseModel.SetState(_baseModel.CapturingState);
            }
        }
    }
}