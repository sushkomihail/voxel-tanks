using UnityEngine;

namespace CustomPhysics
{
    public class CustomWheelCollider : MonoBehaviour
    {
        [SerializeField] private float _suspensionRestDistance = 0.5f;
        [SerializeField] private float _springStrength = 100f;
        [SerializeField] private float _springDamper = 10f;
        [SerializeField] private float _wheelRadius = 0.3f;
        [SerializeField] private float _wheelMass = 20f;
        [SerializeField] private float _brakePadsFriction = 0.8f;
        [SerializeField] private float _gripFactor = 0.7f; // [0, 1]
    
        private Rigidbody _vehicleRigidbody;
        private bool _isGrounded;
    
        private RaycastHit _groundHit;

        private void Awake()
        {
            _vehicleRigidbody = transform.root.GetComponent<Rigidbody>();

            if (!_vehicleRigidbody)
            {
                Debug.LogError("Rigidbody not found: CustomWheelCollider requires Rigidbody on the root GameObject");
            }
        }
    
        private void FixedUpdate()
        {
            UpdateWheelPhysics();
        }
    
        private void UpdateWheelPhysics()
        {
            if (!_vehicleRigidbody) return;
            
            _isGrounded = Physics.Raycast(transform.position, -transform.up, 
                out _groundHit, _suspensionRestDistance + _wheelRadius);
        
            if (_isGrounded)
            {
                Vector3 suspensionForce = CalculateSuspensionForce();
                Vector3 frictionForce = CalculateFrictionForce();
                _vehicleRigidbody.AddForceAtPosition(suspensionForce + frictionForce, transform.position);
            }
        }
    
        private Vector3 CalculateSuspensionForce()
        {
            float compression = _suspensionRestDistance + _wheelRadius - _groundHit.distance;
            float springForce = _springStrength * compression;
        
            Vector3 worldCompressionVelocity = _vehicleRigidbody.GetPointVelocity(transform.position);
            float relativeCompressionSpeed = Vector3.Dot(worldCompressionVelocity, -transform.up);
            float damperForce = _springDamper * relativeCompressionSpeed;
        
            return transform.up * ((springForce + damperForce) * _vehicleRigidbody.mass);
        }
    
        private Vector3 CalculateFrictionForce()
        {
            Vector3 velocity = _vehicleRigidbody.GetPointVelocity(_groundHit.point);
        
            float sidewaysSpeed = Vector3.Dot(velocity, transform.right);
            float desiredSidewaysSpeed = -sidewaysSpeed * _gripFactor;
            float desiredSidewaysAcceleration = desiredSidewaysSpeed / Time.fixedDeltaTime;
            
            Vector3 sidewaysForce = transform.right * (desiredSidewaysAcceleration * _wheelMass);
            return sidewaysForce;
        }
    
        public void ApplyMotorTorque(float torque)
        {
            if (!_isGrounded) return;
        
            Vector3 force = transform.forward * torque / _wheelRadius;
            _vehicleRigidbody.AddForceAtPosition(force, _groundHit.point);
        }
    
        public void ApplyBrakeTorque(float torque)
        {
            if (!_isGrounded) return;
            
            Vector3 force = transform.forward * torque / _wheelRadius;
            _vehicleRigidbody.AddForceAtPosition(force * (_brakePadsFriction * 2), _groundHit.point);
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 rayStart = transform.position;
            Vector3 rayEnd = rayStart - transform.up * (_suspensionRestDistance + _wheelRadius);
        
            Gizmos.DrawLine(rayStart, rayEnd);
        
            if (_isGrounded)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_groundHit.point, 0.05f);
                Gizmos.DrawLine(_groundHit.point, _groundHit.point + _groundHit.normal * 0.2f);
            }
        }
    }
}