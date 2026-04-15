using Tank.Data;
using UnityEngine;

namespace Tank.Modules.Engine
{
    public class Engine : TankModule<EngineData>
    {
        private const float HpToKw = 0.7355f;

        private EngineState _state;
        private float _torqueRate;

        public float DamagedTorqueRate => _data.DamagedTorqueRate;
        public float Torque => _torqueRate * _data.Power * HpToKw * 9550 / _data.RPM;

        public void SetTorqueRate(float torqueRate)
        {
            _torqueRate = torqueRate;
        }

        public override void EnterNormalState()
        {
            base.EnterNormalState();
            _state = new NormalEngineState(this);
            _state.Enter();
        }

        protected override void EnterDamagedState()
        {
            base.EnterDamagedState();
            _state = new DamagedEngineState(this);
            _state.Enter();
        }

        protected override void EnterCriticalState()
        {
            base.EnterCriticalState();
            _state = new CriticalEngineState(this);
            _state.Enter();
        }

        private bool IsBurning()
        {
            float probability = Random.Range(0f, 1f);
            return probability <= _data.FireProbability;
        }
    }
}