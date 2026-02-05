using Extensions;
using Input;
using Tank.Modules;
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
        [SerializeField] private float _rotationSpeed = 60f; // deg/s
        
        private Rigidbody _tankRigidbody;

        public void Init()
        {
            _tankRigidbody = transform.root.GetComponent<Rigidbody>();
            
            _engine.Init();
            _leftTrack.Init();
            _rightTrack.Init();
        }

        public void Move()
        {
            if (!_tankRigidbody) return;
            
            Vector2 moveInputVector = PlayerInput.Instance.GetMoveInputVector();
            float speed = Vector3.Dot(transform.forward, _tankRigidbody.linearVelocity);
            float breakTorque = _engine.Torque * -speed.Sign();

            if (moveInputVector.y == 0)
            {
                if (Mathf.Abs(speed) > 0.05f)
                {
                    _leftTrack.ApplyTorque(breakTorque);
                    _rightTrack.ApplyTorque(breakTorque);
                    return;
                }
                
                _leftTrack.ApplyTorque(0);
                _rightTrack.ApplyTorque(0);
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

        public void Rotate()
        {
            if (!_tankRigidbody) return;
            
            Vector2 moveInputVector = PlayerInput.Instance.GetMoveInputVector();
            float xInputAxis = moveInputVector.y < 0 ? -moveInputVector.x : moveInputVector.x;
            Vector3 deltaRotationAngles = transform.up * (xInputAxis * _rotationSpeed * Time.deltaTime);
            transform.Rotate(deltaRotationAngles);

            // Vector3 angularVelocity = _tankRigidbody.angularVelocity;
            // angularVelocity.y = _rotationSpeed * Mathf.Deg2Rad * xInputAxis;
            // _tankRigidbody.angularVelocity = angularVelocity;
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