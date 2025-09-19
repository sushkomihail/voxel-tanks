using Extensions;
using Input;
using Tank.Modules;
using Tank.Modules.Track;
using Tank.Modules.Transmission;
using UnityEngine;

namespace Tank
{
    public class TankChassis : MonoBehaviour
    {
        [SerializeField] private Engine _engine;
        [SerializeField] private Transmission _transmission;
        [SerializeField] private Track _leftTrack;
        [SerializeField] private Track _rightTrack;
        [SerializeField] private float _rotationSpeed = 60f; // deg/s
        
        private Rigidbody _tankRigidbody;

        public void Init()
        {
            _tankRigidbody = transform.root.GetComponent<Rigidbody>();

            if (!_tankRigidbody)
            {
                Debug.LogError("Rigidbody not found: TankChassis requires Rigidbody on the root GameObject");
                return;
            }
            
            _engine.Init();
            _transmission.Init();
            _leftTrack.Init();
            _rightTrack.Init();
        }

        public void Move()
        {
            Vector2 moveInputVector = PlayerInput.Instance.GetMoveInputVector();
            float speed = Vector3.Dot(transform.forward, _tankRigidbody.linearVelocity);
            float breakTorque = _engine.Torque * -speed.Sign();

            if (moveInputVector.y == 0)
            {
                if (Mathf.Abs(speed) > 0.05f)
                {
                    _leftTrack.ApplyBrakeTorque(breakTorque);
                    _rightTrack.ApplyBrakeTorque(breakTorque);
                    return;
                }
                
                _leftTrack.ApplyMotorTorque(0);
                _rightTrack.ApplyMotorTorque(0);
            }
            else
            {
                if (speed.Sign() != moveInputVector.y.Sign() && speed.Sign() != 0)
                {
                    _leftTrack.ApplyBrakeTorque(breakTorque);
                    _rightTrack.ApplyBrakeTorque(breakTorque);
                    return;
                }
                
                float motorTorque = moveInputVector.y * _engine.Torque;
                _leftTrack.ApplyMotorTorque(motorTorque);
                _rightTrack.ApplyMotorTorque(motorTorque);
            }
        }

        public void Rotate()
        {
            Vector2 moveInputVector = PlayerInput.Instance.GetMoveInputVector();
            float xInputAxis = moveInputVector.y < 0 ? -moveInputVector.x : moveInputVector.x;
            Vector3 deltaRotationAngles = transform.up * (xInputAxis * _rotationSpeed * Time.deltaTime);
            transform.Rotate(deltaRotationAngles);
        }
        
        private void LimitSpeed()
        {
            // if (_tankRigidbody.linearVelocity.magnitude > maxSpeed)
            // {
            //     _tankRigidbody.linearVelocity = _tankRigidbody.linearVelocity.normalized * maxSpeed;
            // }
        }
    }
}