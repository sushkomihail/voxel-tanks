using System.Collections;
using Tank;
using TMPro;
using UnityEngine;

namespace UI
{
    // TODO: Move to TankView
    public class TankChassisView : MonoBehaviour
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
                _linearSpeedText.text = ((int)_smoothedLinearSpeed).ToString();
                _angularSpeedText.text = ((int)_smoothedAngularSpeed).ToString();
                _currentGearText.text = _chassis.CurrentGear.ToString();
                yield return waitForSeconds;
            }
        }
    }
}