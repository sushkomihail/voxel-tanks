using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class EngineData : TankModuleData
    {
        [SerializeField] private float _power = 700f;
        [SerializeField] private float _rpm = 3000f;
        [SerializeField] private float _damagedTorqueRate = 0.5f;
        [SerializeField] private float _fireProbability = 0.15f;
        
        public float Power => _power;
        public float RPM => _rpm;
        public float DamagedTorqueRate => _damagedTorqueRate;
        public float FireProbability => _fireProbability;
    }
}