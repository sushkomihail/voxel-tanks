using System.Collections;
using Tank;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DrivingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _linearSpeedText;
        [SerializeField] private TMP_Text _angularSpeedText;
        [SerializeField] private TMP_Text _currentGearText;
        [SerializeField] private float _smoothSpeed = 4f;
        [SerializeField] private float _updateInterval = 0.1f;

        private TankChassis _chassis;
        private float _smoothedLinearSpeed;
        private float _smoothedAngularSpeed;
        
        public void Initialize(TankChassis chassis)
        {
            _chassis = chassis;
            StartCoroutine(UpdateWithInterval());
        }

        private void FixedUpdate()
        {
            _smoothedLinearSpeed = Mathf.Lerp(_smoothedLinearSpeed, _chassis.LinearSpeed, _smoothSpeed * Time.fixedDeltaTime);
            _smoothedAngularSpeed = Mathf.Lerp(_smoothedAngularSpeed, _chassis.AngularSpeed, _smoothSpeed * Time.fixedDeltaTime);
        }

        // TODO: Make end loop
        private IEnumerator UpdateWithInterval()
        {
            var waitForSeconds = new WaitForSeconds(_updateInterval);

            while (true)
            {
                _linearSpeedText.text = $"Linear Speed\n{(int)_smoothedLinearSpeed} km/h";
                _angularSpeedText.text = $"Angular Speed\n{(int)_smoothedAngularSpeed} deg/s";
                _currentGearText.text = $"Gear\n{_chassis.CurrentGear}";
                yield return waitForSeconds;
            }
        }
    }
}