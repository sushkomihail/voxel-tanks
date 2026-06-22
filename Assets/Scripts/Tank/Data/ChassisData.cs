using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class ChassisData
    {
        [SerializeField] private float _brakeTorque = 5000f;
        [SerializeField] private float _maxForwardSpeed = 50f;
        [SerializeField] private float _maxBackwardSpeed = 20f;
        [SerializeField] private float _rotationSpeed = 60f; // deg/s
        
        public float BrakeTorque => _brakeTorque;
        public float MaxForwardSpeed => _maxForwardSpeed;
        public float MaxBackwardSpeed => _maxBackwardSpeed;
        public float RotationSpeed => _rotationSpeed;
    }
}