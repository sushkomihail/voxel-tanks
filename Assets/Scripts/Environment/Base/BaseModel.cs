using System;
using System.Collections.Generic;
using Input;
using Tank;
using UnityEngine;

namespace Environment.Base
{
    public class BaseModel : MonoBehaviour, IRouterTarget
    {
        [SerializeField] private float _radius = 3;
        [SerializeField] private float _captureTime = 10;

        public event Action OnCapturingProgressChanged;
        
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
        public bool IsPlayerInBase => _playerId == OwnerId;
        public string OwnerId { get; private set; }
        public string PreviousOwnerId { get; private set; }
        
        private List<TankController> _onFieldTanks;
        private readonly Dictionary<string, bool> _insideBaseTanks =  new();

        private string _playerId;
        private BaseState _state;

        public void Initialize(List<TankController> onFieldTanks, string playerId)
        {
            _onFieldTanks = onFieldTanks;
            _playerId = playerId;

            foreach (TankController tank in _onFieldTanks)
            {
                _insideBaseTanks.Add(tank.BattleData.Id, false);
                tank.Health.OnDeath += () =>
                {
                    _onFieldTanks.Remove(tank);
                    _insideBaseTanks.Remove(tank.BattleData.Id);
                };
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
            int count = 0;
            
            foreach (bool isInsideBase in _insideBaseTanks.Values)
            {
                count += isInsideBase ? 1 : 0;
            }
            
            return count;
        }

        public bool TryGetFirstInsideBaseTankId(out string id)
        {
            id = "";
            
            foreach (var (key, value) in _insideBaseTanks)
            {
                if (value)
                {
                    id = key;
                    return true;
                }
            }
            
            return false;
        }

        public bool IsPositionInside(Vector3 position)
        {
            float sqrDistance = (position - transform.position).sqrMagnitude;
            return sqrDistance <= _radius * _radius;
        }

        public void SetState(BaseState state)
        {
            _state = state;
            _state.Enter();
        }

        public void SetOwnerId(string newOwnerId)
        {
            PreviousOwnerId = OwnerId;
            OwnerId = newOwnerId;
        }

        public void SetIsTankInsideBase(string tankId, bool isInsideBase)
        {
            _insideBaseTanks[tankId] = isInsideBase;
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
            foreach (TankController tank in _onFieldTanks)
            {
                if (IsPositionInside(tank.transform.position) && !_insideBaseTanks[tank.BattleData.Id])
                {
                    _insideBaseTanks[tank.BattleData.Id] = true;
                    _state.OnTankEntersBase(tank.BattleData);
                    continue;
                }
                
                if (!IsPositionInside(tank.transform.position) && _insideBaseTanks[tank.BattleData.Id])
                {
                    _insideBaseTanks[tank.BattleData.Id] = false;
                    _state.OnTankLeavesBase(tank.BattleData);
                }
            }
        }

        private void ClampCaptureProgress()
        {
            CapturingProgress = Mathf.Clamp(CapturingProgress, 0f, CaptureTime);
        }
    }
}
