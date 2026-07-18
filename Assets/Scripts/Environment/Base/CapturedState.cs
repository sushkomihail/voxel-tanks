using System;
using Tank;

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
            _baseModel.UpgradeManager.Heal(_baseModel.OwnerController.Health);
            _baseModel.UpgradeManager.ProvideRepairKits(_baseModel.OwnerController.Equipment);
            _baseModel.UpgradeManager.EnqueueUpgradePair();
            OnCaptured?.Invoke();
        }

        public override void OnTankEntersBase(TankController tankController)
        {
            base.OnTankEntersBase(tankController);
            
            if (_baseModel.GetInsideBaseTanksCount() == 1 && tankController != _baseModel.OwnerController)
            {
                _baseModel.SetOwnerController(tankController);
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