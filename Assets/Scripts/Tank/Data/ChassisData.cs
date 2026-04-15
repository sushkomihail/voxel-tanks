using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class ChassisData
    {
        [SerializeField] private float _brakeTorque = 5000f;
        [SerializeField] private float _rotationSpeed = 60f; // deg/s
        
        public float BrakeTorque => _brakeTorque;
        public float RotationSpeed => _rotationSpeed;
    }
}