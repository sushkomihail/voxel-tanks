using CustomPhysics;
using UnityEngine;

namespace Tank.Modules.Track
{
    public class Track : TankModule
    {
        [SerializeField] private CustomWheelCollider[] _wheels;

        private TrackState _state;
        private float _torqueRate; // [0, 1]

        public override void Init()
        {
            base.Init();
            SetTorqueRate(1f);
        }
        
        public void SetTorqueRate(float torqueRate)
        {
            _torqueRate = torqueRate;
        }
        
        public void ApplyMotorTorque(float torque)
        {
            foreach (var wheel in _wheels)
            {
                wheel.ApplyMotorTorque(torque * _torqueRate / _wheels.Length);
            }
        }

        public void ApplyBrakeTorque(float torque)
        {
            foreach (var wheel in _wheels)
            {
                wheel.ApplyBrakeTorque(torque * _torqueRate / _wheels.Length);
            }
        }

        public override void EnterDamagedState()
        {
            _state = new DamagedTrackState(this);
            _state.Enter();
        }

        public override void EnterCriticalState()
        {
            _state = new CriticalTrackState(this);
            _state.Enter();
        }
    }
}