using System;
using UnityEngine;

namespace Tank.Modules.Transmission
{
    [Serializable]
    public class Transmission
    {
        [SerializeField] private float _maxForwardSpeed = 60f; // kph
        [SerializeField] private float _maxBackwardSpeed = 20f; // kph

        private const float FirstGearSpeed = 5f; // kph
        private const float SecondGearSpeed = 20f; // kph
        
        private Gear[] _gears; 
        private int _gearIndex;

        public void Init()
        {
            _gears = new[]
            {
                new Gear(_maxForwardSpeed),
                new Gear(SecondGearSpeed <= _maxForwardSpeed ? SecondGearSpeed : _maxForwardSpeed),
                new Gear(FirstGearSpeed <= _maxForwardSpeed ? FirstGearSpeed : _maxForwardSpeed),
                new Gear(0f),
                new Gear(_maxBackwardSpeed, true)
            };
            _gearIndex = 3;
        }

        public float GetSpeedLimit()
        {
            return _gears[_gearIndex].SpeedLimit / 3.6f;
        }

        public void GearUp()
        {
            if (_gearIndex == 0) return;

            _gearIndex--;
        }

        public void GearDown()
        {
            if (_gearIndex == _gears.Length - 1) return;
            
            _gearIndex++;
        }

        public void GearUpToHighest()
        {
            _gearIndex = 0;
        }

        public void GearDownToLowest()
        {
            _gearIndex = _gears.Length - 1;
        }
    }
}