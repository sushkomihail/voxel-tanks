using Tank.Data;
using UnityEngine;
using UpgradeSystem;

namespace Vehicles
{
    public class Engine
    {
        private const float HpToKw = 0.7355f;
        
        private readonly EngineData _data;
        private readonly UpgradeBroker _upgradeBroker;
        private readonly object _owner;
        
        public float ShiftUpRpm => _data.ShiftUpRpm;
        public float ShiftDownRpm => _data.ShiftDownRpm;

        private float Power
        {
            get
            {
                StatQuery query = new StatQuery(StatType.EnginePower, _data.Power);
                _upgradeBroker.Query(_owner, query);
                return query.Value;
            }
        }

        public Engine(EngineData data, UpgradeBroker upgradeBroker, object owner)
        {
            _data = data;
            _upgradeBroker = upgradeBroker;
            _owner = owner;
        }
        
        public float CalculateWheelTorque(float rpm, float mainGearRatio, float currentGearRatio)
        {
            return Power * HpToKw * 9550 / rpm * mainGearRatio * currentGearRatio;
        }

        public float CalculateRpm(float wheelRpm, float mainGearRatio, float currentGearRatio)
        {
            float rpm = wheelRpm * mainGearRatio * currentGearRatio;
            return Mathf.Clamp(rpm, _data.MinRpm, _data.MaxRpm);
        }
    }
}