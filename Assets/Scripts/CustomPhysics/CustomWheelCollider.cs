using UnityEngine;

namespace CustomPhysics
{
    public class CustomWheelCollider : MonoBehaviour
    {
        [SerializeField] private float _suspensionRestDistance = 0.5f;
        [SerializeField] private float _springStrength = 100f;
        [SerializeField] private float _springDamper = 10f;
        [SerializeField] private float _wheelRadius = 0.3f;
        [SerializeField] private float _massPerWheel = 150f;
        [SerializeField] private float _gripFactor = 0.7f; // [0, 1]
        [SerializeField] private LayerMask _groundLayer = 1 << 3;
    
        private Rigidbody _vehicleRigidbody;
        private RaycastHit _groundHit;
        private float _currentGripFactor;
        
        public bool IsGrounded { get; private set; }
        public float RPM => 30 * _vehicleRigidbody.linearVelocity.magnitude / (Mathf.PI * _wheelRadius);

        private void Awake()
        {
            _vehicleRigidbody = transform.root.GetComponent<Rigidbody>();
            _currentGripFactor = _gripFactor;
        }
    
        private void FixedUpdate()
        {
            UpdateWheelPhysics();
        }

        public void SetGripFactor(float gripFactor)
        {
            _currentGripFactor = gripFactor;
        }
    
        private void UpdateWheelPhysics()
        {
            if (!_vehicleRigidbody) return;
            
            IsGrounded = Physics.Raycast(transform.position, -transform.up, 
                out _groundHit, _suspensionRestDistance + _wheelRadius, _groundLayer.value);
        
            if (IsGrounded)
            {
                Vector3 suspensionForce = CalculateSuspensionForce();
                Vector3 frictionForce = CalculateFrictionForce();
                Vector3 totalForce = suspensionForce + frictionForce;
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
            float desiredSidewaysSpeed = -sidewaysSpeed * _currentGripFactor;
            float desiredSidewaysAcceleration = desiredSidewaysSpeed / Time.fixedDeltaTime;
            
            Vector3 sidewaysForce = transform.right * (desiredSidewaysAcceleration * _massPerWheel);
            return sidewaysForce;
        }
    
        public void ApplyTorque(float torque)
        {
            if (!IsGrounded) return;
        
            Vector3 longitudinalForce = transform.forward * torque / _wheelRadius;
            _vehicleRigidbody.AddForceAtPosition(longitudinalForce, transform.position);
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 rayStart = transform.position;
            Vector3 rayEnd = rayStart - transform.up * (_suspensionRestDistance + _wheelRadius);
        
            Gizmos.DrawLine(rayStart, rayEnd);
        
            if (IsGrounded)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_groundHit.point, 0.05f);
                Gizmos.DrawLine(_groundHit.point, _groundHit.point + _groundHit.normal * 0.2f);
            }
        }
    }
}