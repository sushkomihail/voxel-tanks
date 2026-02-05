using CustomPhysics;
using UnityEngine;

namespace Tank.Modules.Track
{
    public class Track : TankModule
    {
        [SerializeField] private CustomWheelCollider[] _wheels;
        [SerializeField] private float _damagedTorqueRate = 0.5f;

        private TrackState _state;
        private float _torqueRate; // [0, 1]
        
        public float DamagedTorqueRate => _damagedTorqueRate;

        public void SetTorqueRate(float torqueRate)
        {
            _torqueRate = torqueRate;
        }
        
        public void ApplyTorque(float torque)
        {
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

        public override void EnterDamagedState()
        {
            base.EnterDamagedState();
            _state = new DamagedTrackState(this);
            _state.Enter();
        }

        public override void EnterCriticalState()
        {
            base.EnterCriticalState();
            _state = new CriticalTrackState(this);
            _state.Enter();
        }
    }
}