using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class GunData
    {
        [SerializeField] private int _caliber;
        [SerializeField] private float _minVerticalAngle = -5f;
        [SerializeField] private float _maxVerticalAngle = 20f;
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField] private float _rotationLag = 0.5f;
        [SerializeField] private LayerMask _aimMask = ~(1 << 6);
        
        public int Caliber => _caliber;
        public float MinVerticalAngle => _minVerticalAngle;
        public float MaxVerticalAngle => _maxVerticalAngle;
        public float RotationSpeed => _rotationSpeed;
        public float RotationLag => _rotationLag;
        public LayerMask AimMask => _aimMask;
    }
}