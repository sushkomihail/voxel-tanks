using System;
using Input;
using Tank.Data;

namespace Environment.Base
{
    public class CapturedState : BaseState
    {
        public static event Action OnCaptured;
        
        public CapturedState(BaseModel baseModelReference) : base(baseModelReference)
        {
        }

        public override void Enter()
        {
            OnCaptured?.Invoke();
        }

        public override void OnTankEntersBase(TankBattleData tankData)
        {
            base.OnTankEntersBase(tankData);
            
            if (_baseModel.GetInsideBaseTanksCount() == 1 && tankData.Id != _baseModel.OwnerId)
            {
                _baseModel.SetOwnerId(tankData.Id);
                _baseModel.SetState(_baseModel.RecapturingState);
            }
        }

        public override void UpdateCaptureProgress()
        {
            if (_baseModel.CapturingProgress < _baseModel.CaptureTime)
            {
                _baseModel.IncreaseCapturingProgress();
            }
        }
    }
}