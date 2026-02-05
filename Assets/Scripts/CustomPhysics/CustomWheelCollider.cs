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
        [SerializeField] private float _gripFactor = 0.7f; // [0, 1]
    
        private Rigidbody _vehicleRigidbody;
        private RaycastHit _groundHit;
        private Vector3 _longitudinalForce;
        private bool _isGrounded;

        private void Awake()
        {
            _vehicleRigidbody = transform.root.GetComponent<Rigidbody>();
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
                Vector3 totalForce = _longitudinalForce + suspensionForce + frictionForce;
                _vehicleRigidbody.AddForceAtPosition(totalForce, transform.position);
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
    
        public void ApplyTorque(float torque)
        {
            if (!_isGrounded) return;
        
            _longitudinalForce = transform.forward * torque / _wheelRadius;
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