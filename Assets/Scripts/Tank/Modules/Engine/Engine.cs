using UnityEngine;

namespace Tank.Modules.Engine
{
    public class Engine : TankModule
    {
        [SerializeField] private float _power = 700f;
        [SerializeField] private float _rpm = 3000f;
        [SerializeField] private float _damagedTorqueRate = 0.5f;
        [SerializeField] private float _fireProbability = 0.15f;

        private const float HpToKw = 0.7355f;

        private EngineState _state;
        private float _torqueRate;

        public float Power => _power;
        public float DamagedTorqueRate => _damagedTorqueRate;
        public float Torque => _torqueRate * _power * HpToKw * 9550 / _rpm;

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

        public override void EnterDamagedState()
        {
            base.EnterDamagedState();
            _state = new DamagedEngineState(this);
            _state.Enter();
        }

        public override void EnterCriticalState()
        {
            base.EnterCriticalState();
            _state = new CriticalEngineState(this);
            _state.Enter();
        }

        // private bool IsBurning()
        // {
        //     float probability = Random.Range(0f, 1f);
        //     return probability <= _fireProbability;
        // }
    }
}