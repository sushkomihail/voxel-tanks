using Extensions;
using Input;
using UnityEngine;

namespace Tank
{
    public class TankChassis : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private float _maxForwardSpeed;
        [SerializeField] private float _maxBackwardSpeed;
        [SerializeField] private float _rotationSpeed;
        
        private Vector2 _movementAxes;
        private float _maxForwardSpeedInMetersPerSecond;
        private float _maxBackwardSpeedInMetersPerSecond;

        public void Initialize()
        {
            _maxForwardSpeedInMetersPerSecond = _maxForwardSpeed / 3.6f;
            _maxBackwardSpeedInMetersPerSecond = _maxBackwardSpeed / 3.6f;
        }

        public void ReadInput(TankInput input)
        {
            _movementAxes = input.GetActions().Movement.ReadValue<Vector2>();
        }
        
        public void Move()
        {
            float dot = Vector3.Dot(_rigidbody.linearVelocity, transform.forward);
            int velocitySign = MathExtension.Sign(dot);
            Vector3 linearForce;
            
            if (_movementAxes.y == 0 && velocitySign != 0)
            {
                linearForce = transform.forward * (-velocitySign * _deceleration);

                if (_rigidbody.linearVelocity.magnitude < 0.5f)
                {
                    _rigidbody.linearVelocity = Vector3.zero;
                }
            }
            else if (velocitySign != _movementAxes.y && velocitySign != 0)
            {
                linearForce = transform.forward * (_movementAxes.y * _deceleration);
            }
            else
            {
                linearForce = transform.forward * (_movementAxes.y * _acceleration);
            }
            
            Vector3 lateralForce = transform.right * 
                Vector3.Dot(_rigidbody.linearVelocity, transform.right) / Time.fixedDeltaTime;
            _rigidbody.AddForce(linearForce - lateralForce, ForceMode.Acceleration);
            ClampSpeed(velocitySign);
        }

        public void Rotate()
        {
            float xInputAxis = _movementAxes.y < 0 ? _movementAxes.x * -1 : _movementAxes.x;
            Vector3 deltaRotationAngles = Vector3.up * (xInputAxis * _rotationSpeed * Time.deltaTime);
            transform.Rotate(deltaRotationAngles);
        }

        private void ClampSpeed(int velocitySign)
        {
            float currentSpeed = _rigidbody.linearVelocity.magnitude * 3.6f;

            if (velocitySign < 0 && currentSpeed >= _maxBackwardSpeed)
            {
                _rigidbody.linearVelocity = transform.forward * -_maxBackwardSpeedInMetersPerSecond;
            }
            else if (velocitySign > 0 && currentSpeed >= _maxForwardSpeed)
            {
                _rigidbody.linearVelocity = transform.forward * _maxForwardSpeedInMetersPerSecond;
            }
        }
    }
}