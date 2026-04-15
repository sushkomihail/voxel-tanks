using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class TurretData
    {
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField] private float _minHorizontalAngle = -180f;
        [SerializeField] private float _maxHorizontalAngle = 180f;
        
        public float RotationSpeed => _rotationSpeed;
        public float MinHorizontalAngle => _minHorizontalAngle;
        public float MaxHorizontalAngle => _maxHorizontalAngle;
    }
}