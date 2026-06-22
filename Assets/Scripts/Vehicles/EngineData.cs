using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class EngineData : TankModuleData
    {
        [SerializeField] private float _power = 700f;
        [SerializeField] private float _minRpm = 1000f;
        [SerializeField] private float _maxRpm = 5000f;
        [SerializeField] private float _shiftUpRpm = 3000f;
        [SerializeField] private float _shiftDownRpm = 1500f;
        [SerializeField] private float _damagedTorqueRate = 0.5f;
        [SerializeField] private float _fireProbability = 0.15f;
        
        public float Power => _power;
        public float MinRpm => _minRpm;
        public float MaxRpm => _maxRpm;
        public float ShiftUpRpm => _shiftUpRpm;
        public float ShiftDownRpm => _shiftDownRpm;
        public float DamagedTorqueRate => _damagedTorqueRate;
        public float FireProbability => _fireProbability;
    }
}