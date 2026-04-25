using Tank.Data;

namespace Environment.Base
{
    public abstract class BaseState
    {
        protected readonly BaseModel _baseModel;

        protected BaseState(BaseModel baseModelReference)
        {
            _baseModel = baseModelReference;
        }
        
        public virtual void Enter() {}
        
        public virtual void OnTankEntersBase(TankBattleData tankData)
        {
            _baseModel.SetIsTankInsideBase(tankData.Id, true);
        }
        
        public virtual void UpdateCaptureProgress() {}

        public virtual void OnTankLeavesBase(TankBattleData tankData)
        {
            _baseModel.SetIsTankInsideBase(tankData.Id, false);
        }
    }
}