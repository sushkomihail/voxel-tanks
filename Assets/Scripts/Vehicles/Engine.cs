using Tank.Data;
using UnityEngine;

namespace Vehicles
{
    public class Engine
    {
        public float ShiftUpRpm => _data.ShiftUpRpm;
        public float ShiftDownRpm => _data.ShiftDownRpm;
        
        private const float HpToKw = 0.7355f;
        
        private readonly EngineData _data;

        public Engine(EngineData data)
        {
            _data = data;
        }
        
        public float CalculateWheelTorque(float rpm, float mainGearRatio, float currentGearRatio)
        {
            return _data.Power * HpToKw * 9550 / rpm * mainGearRatio * currentGearRatio;
        }

        public float CalculateRpm(float wheelRpm, float mainGearRatio, float currentGearRatio)
        {
            float rpm = wheelRpm * mainGearRatio * currentGearRatio;
            return Mathf.Clamp(rpm, _data.MinRpm, _data.MaxRpm);
        }
    }
}