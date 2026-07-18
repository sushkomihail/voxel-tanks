using System;
using System.Collections.Generic;
using System.Linq;
using Navigation;
using Tank;
using UnityEngine;
using UpgradeSystem;
using VContainer;

namespace Environment.Base
{
    public class BaseModel : MonoBehaviour, IRouterTarget
    {
        [SerializeField] private float _radius = 3;
        [SerializeField] private float _captureTime = 10;

        public event Action OnCapturingProgressChanged;
        
        public Vector3 Position => transform.position;
        public bool IsActive { get; private set; }
        
        public float CaptureTime => _captureTime;
        public float CapturingProgress { get; private set; }
        public float CapturingRate
        {
            get
            {
                if (_captureTime <= 0) return 0;
                return CapturingProgress / _captureTime;
            }
        }

        public NotCapturedState NotCapturedState { get; private set; }
        public CapturingState CapturingState { get; private set; }
        public CapturingPauseState CapturingPauseState { get; private set; }
        public CapturedState CapturedState { get; private set; }
        public RecapturingState RecapturingState { get; private set; }
        public RecapturingPauseState RecapturingPauseState { get; private set; }
        
        public bool IsCapturing => _state is CapturingState;
        public bool IsRecapturing => _state is RecapturingState;
        public bool IsPlayerInBase => _playerController == OwnerController;
        public TankController OwnerController { get; private set; }
        public TankController PreviousOwnerController { get; private set; }
        public UpgradeManager UpgradeManager { get; private set; }
        
        private readonly Dictionary<TankController, bool> _insideBaseTanks =  new();
        private List<TankController> _onFieldTanks; // Maybe delete
        private TankController _playerController;
        private BaseState _state;

        [Inject]
        public void Construct(UpgradeManager upgradeManager)
        {
            UpgradeManager = upgradeManager;
        }

        public void Initialize(List<TankController> onFieldTanks, TankController playerController)
        {
            IsActive = true;
            
            _onFieldTanks = onFieldTanks;
            _playerController = playerController;

            foreach (TankController tankController in _onFieldTanks)
            {
                _insideBaseTanks.Add(tankController, false);
            }
            
            NotCapturedState = new NotCapturedState(this);
            CapturingState = new CapturingState(this);
            CapturingPauseState = new CapturingPauseState(this);
            CapturedState = new CapturedState(this);
            RecapturingState = new RecapturingState(this);
            RecapturingPauseState = new RecapturingPauseState(this);
            
            SetState(NotCapturedState);
        }

        private void Update()
        {
            ObserveOnFieldTanks();
            _state.UpdateCaptureProgress();
        }

        public int GetInsideBaseTanksCount()
        {
            return _insideBaseTanks.Values.Count(v => v);
        }

        public bool TryGetFirstInsideBaseTankController(out TankController controller)
        {
            controller = null;
            
            foreach (var (tankController, isInsideBase) in _insideBaseTanks)
            {
                if (isInsideBase)
                {
                    controller = tankController;
                    return true;
                }
            }
            
            return false;
        }

        public void SetState(BaseState state)
        {
            _state = state;
            _state.Enter();
        }

        public void SetOwnerController(TankController newOwnerController)
        {
            PreviousOwnerController = OwnerController;
            OwnerController = newOwnerController;
        }

        public void SetIsTankInsideBase(TankController tankController, bool isInsideBase)
        {
            _insideBaseTanks[tankController] = isInsideBase;
        }

        public void IncreaseCapturingProgress()
        {
            CapturingProgress += Time.deltaTime;
            ClampCaptureProgress();
            OnCapturingProgressChanged?.Invoke();
        }

        public void DecreaseCapturingProgress()
        {
            CapturingProgress -= Time.deltaTime;
            ClampCaptureProgress();
            OnCapturingProgressChanged?.Invoke();
        }
        
        private void ObserveOnFieldTanks()
        {
            foreach (TankController tankController in _onFieldTanks)
            {
                if (!tankController.IsActive)
                {
                    _insideBaseTanks[tankController] = false;
                    continue;
                }
                
                if (IsPositionInside(tankController.transform.position) && !_insideBaseTanks[tankController])
                {
                    _insideBaseTanks[tankController] = true;
                    _state.OnTankEntersBase(tankController);
                    continue;
                }
                
                if (!IsPositionInside(tankController.transform.position) && _insideBaseTanks[tankController])
                {
                    _insideBaseTanks[tankController] = false;
                    _state.OnTankLeavesBase(tankController);
                }
            }
        }

        private bool IsPositionInside(Vector3 position)
        {
            float sqrDistance = (position - transform.position).sqrMagnitude;
            return sqrDistance <= _radius * _radius;
        }

        private void ClampCaptureProgress()
        {
            CapturingProgress = Mathf.Clamp(CapturingProgress, 0f, CaptureTime);
        }
    }
}
