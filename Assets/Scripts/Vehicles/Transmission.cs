using UnityEngine;

namespace Vehicles
{
    public class Transmission
    {
        public float MainGearRatio => _data.MainGearRatio;
        public int CurrentGear { get; private set; }
        
        private readonly TransmissionData _data;
        
        public Transmission(TransmissionData data)
        {
            _data = data;
        }
        
        public float GetCurrentGearRatio()
        {
            if (CurrentGear > 0) return _data.ForwardGearRatios[CurrentGear - 1];
            if (CurrentGear < 0) return _data.ReverseGearRatios[Mathf.Abs(CurrentGear) - 1];
            return 0f;
        }

        public void ShiftUp()
        {
            if (CurrentGear < _data.ForwardGearRatios.Count)
            {
                CurrentGear++;
            }
        }

        public void ShiftDown()
        {
            if (CurrentGear > -_data.ReverseGearRatios.Count)
            {
                CurrentGear--;
            }
        }

        public void ShiftAutomatically(Vector2 inputVector, float engineRpm, float shiftUpRpm, float shiftDownRpm)
        {
            if (inputVector.y > 0 && CurrentGear <= 0) CurrentGear = 1;
            else if (inputVector.y < 0 && CurrentGear >= 0) CurrentGear = -1;
            else if (inputVector.x != 0 && CurrentGear == 0) CurrentGear = 1;
            else if (inputVector == Vector2.zero) CurrentGear = 0;

            if (CurrentGear > 0)
            {
                if (engineRpm > shiftUpRpm) ShiftUp();
                else if (engineRpm < shiftDownRpm && CurrentGear > 1) ShiftDown();
            }
            else if (CurrentGear < 0)
            {
                if (engineRpm > shiftUpRpm) ShiftDown();
                else if (engineRpm < shiftDownRpm && CurrentGear < -1) ShiftUp();
            }
        }
    }
}
