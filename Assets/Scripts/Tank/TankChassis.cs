using Extensions;
using Tank.Modules.Engine;
using Tank.Modules.Track;
using UnityEngine;

namespace Tank
{
    public class TankChassis : MonoBehaviour
    {
        [SerializeField] private Engine _engine;
        [SerializeField] private Track _leftTrack;
        [SerializeField] private Track _rightTrack;
        [SerializeField] private float _brakeTorque = 3000f;
        [SerializeField] private float _rotationSpeed = 60f; // deg/s

        private const float StopThreshold = 0.2f;
        
        private Rigidbody _tankRigidbody;

        public void Init()
        {
            _tankRigidbody = transform.root.GetComponent<Rigidbody>();
            
            _engine.Init();
            _leftTrack.Init();
            _rightTrack.Init();
        }

        public void Move(Vector2 moveInputVector)
        {
            if (!_tankRigidbody) return;
            
            float speed = Vector3.Dot(transform.forward, _tankRigidbody.linearVelocity);
            float breakTorque = _brakeTorque * -speed.Sign();

            if (moveInputVector.y == 0)
            {
                float absSpeed = Mathf.Abs(speed);
                
                if (absSpeed > StopThreshold)
                {
                    _leftTrack.ApplyTorque(breakTorque);
                    _rightTrack.ApplyTorque(breakTorque);
                }
                
                if (absSpeed <= StopThreshold && _leftTrack.IsGrounded() && _rightTrack.IsGrounded())
                { 
                    _tankRigidbody.linearVelocity = Vector3.zero;
                }
            }
            else
            {
                if (speed.Sign() != moveInputVector.y.Sign() && speed.Sign() != 0)
                {
                    _leftTrack.ApplyTorque(breakTorque);
                    _rightTrack.ApplyTorque(breakTorque);
                    return;
                }
                
                float motorTorque = moveInputVector.y * _engine.Torque;
                _leftTrack.ApplyTorque(motorTorque);
                _rightTrack.ApplyTorque(motorTorque);
            }
        }

        public void Rotate(Vector2 moveInputVector)
        {
            if (!_tankRigidbody) return;
            
            float xInputAxis = moveInputVector.y < 0 ? -moveInputVector.x : moveInputVector.x;
            Vector3 angularVelocity = _tankRigidbody.angularVelocity;
            angularVelocity.y = _rotationSpeed * Mathf.Deg2Rad * xInputAxis;
            _tankRigidbody.angularVelocity = angularVelocity;
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