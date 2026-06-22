using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vehicles
{
    [Serializable]
    public class TransmissionData
    {
        [SerializeField] private float[] _forwardGearRatios = { 3.5f, 1.8f, 1f };
        [SerializeField] private float[] _reverseGearRatios = { 3f, 1.4f };
        [SerializeField] private float _mainGearRatio = 9.8f;
        
        public IReadOnlyList<float> ForwardGearRatios => _forwardGearRatios;
        public IReadOnlyList<float> ReverseGearRatios => _reverseGearRatios;
        public float MainGearRatio => _mainGearRatio;
    }
}