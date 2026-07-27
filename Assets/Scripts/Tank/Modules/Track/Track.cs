using CustomPhysics;
using Tank.Data;
using UnityEngine;

namespace Tank.Modules.Track
{
    public class Track : TankModule
    {
        [SerializeField] private CustomWheelCollider[] _wheels;

        public override TankModuleType Type => TankModuleType.Track;
        public int WheelsCount => _wheels.Length;
        public float DamagedTorqueRate
        {
            get
            {
                if (_data is TrackData trackData)
                {
                    return trackData.DamagedTorqueRate;
                }
                
                return 0f;
            }
        }

        private TrackState _state;
        private float _torqueRate; // [0, 1]

        public void SetGripFactor(float gripFactor)
        {
            foreach (CustomWheelCollider wheel in _wheels)
            {
                wheel.SetGripFactor(gripFactor);
            }
        }
        
        public void SetTorqueRate(float torqueRate)
        {
            _torqueRate = torqueRate;
        }

        public float CalculateAvgWheelRpm()
        {
            float rpm = 0;
            
            foreach (CustomWheelCollider wheel in _wheels)
            {
                rpm += wheel.RPM;
            }
            
            return rpm;
        }

        public bool IsGrounded()
        {
            foreach (var wheel in _wheels)
            {
                if (!wheel.IsGrounded) return false;
            }
            
            return true;
        }
        
        public void ApplyTorque(float torque)
        {
            if (!IsGrounded()) return;
            
            foreach (var wheel in _wheels)
            {
                wheel.ApplyTorque(torque * _torqueRate / _wheels.Length);
            }
        }

        public override void EnterNormalState()
        {
            base.EnterNormalState();
            _state = new NormalTrackState(this);
            _state.Enter();
        }

        protected override void EnterDamagedState()
        {
            base.EnterDamagedState();
            _state = new DamagedTrackState(this);
            _state.Enter();
        }

        protected override void EnterCriticalState()
        {
            base.EnterCriticalState();
            _state = new CriticalTrackState(this);
            _state.Enter();
        }
    }
}