using ArmorSystem;
using Extensions;
using Tank.Data;
using Tank.Modules.Track;
using UnityEngine;
using VContainer;
using Vehicles;

namespace Tank
{
    public class TankChassis : MonoBehaviour
    {
        [SerializeField] private Track _leftTrack;
        [SerializeField] private Track _rightTrack;
        [SerializeField] private AnimationCurve _gripFactorCurve;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _hullColliderPrefab;
        [SerializeField] private Transform _trackColliderPrefab;

        public float LinearSpeed { get; private set; }
        public float AngularSpeed { get; private set; }
        public int CurrentGear => _transmission.CurrentGear;
        
        private const float StopThreshold = 0.2f;
        private const float MpsToKph = 3.6f;
        
        private CollidersUpdater _collidersUpdater;
        private Rigidbody _tankRigidbody;
        private ChassisData _data;
        private Engine _engine;
        private Transmission _transmission;
        
        [Inject]
        public void Construct(CollidersUpdater collidersUpdater)
        {
            _collidersUpdater = collidersUpdater;
        }

        public void Initialize(ChassisData chassisData, EngineData engineData, TransmissionData transmissionData,
            TrackData trackData, TankHealth health)
        {
            _tankRigidbody = transform.root.GetComponent<Rigidbody>();
            _tankRigidbody.centerOfMass = _centerOfMass.localPosition;
            _data = chassisData;
            
            _engine = new Engine(engineData);
            _transmission = new Transmission(transmissionData);
            _leftTrack.Initialize(trackData);
            _rightTrack.Initialize(trackData);
            
            InitializeColliders(health);
        }

        public void HandleMovement(Vector2 moveInputVector)
        {
            if (!_tankRigidbody) return;
            
            float wheelRpm = CalculateAvgWheelRpm();
            float engineRpm = 
                _engine.CalculateRpm(wheelRpm, _transmission.MainGearRatio, _transmission.GetCurrentGearRatio());
            float wheelTorque = 
                _engine.CalculateWheelTorque(engineRpm, _transmission.MainGearRatio, _transmission.GetCurrentGearRatio());
            
            _transmission.ShiftAutomatically(moveInputVector, engineRpm, _engine.ShiftUpRpm, _engine.ShiftDownRpm);
            
            HandleLinearMovement(moveInputVector.y, wheelTorque);
            HandleAngularMovement(moveInputVector, wheelTorque);
        }

        private void HandleLinearMovement(float linearInput, float wheelTorque)
        {
            _leftTrack.SetGripFactor(_gripFactorCurve.keys[0].value);
            _rightTrack.SetGripFactor(_gripFactorCurve.keys[0].value);
            
            float speed = Vector3.Dot(transform.forward, _tankRigidbody.linearVelocity);
            float breakTorque = _data.BrakeTorque * -speed.Sign();
            
            LinearSpeed = Mathf.Abs(speed) * MpsToKph;

            if (linearInput == 0)
            {
                if (Mathf.Abs(speed) > StopThreshold)
                {
                    _leftTrack.ApplyTorque(breakTorque);
                    _rightTrack.ApplyTorque(breakTorque);
                }
                else if (_leftTrack.IsGrounded() && _rightTrack.IsGrounded())
                {
                    _tankRigidbody.linearVelocity = Vector3.zero;
                }
            }
            else
            {
                if (speed.Sign() != linearInput.Sign() && speed.Sign() != 0)
                {
                    _leftTrack.ApplyTorque(breakTorque);
                    _rightTrack.ApplyTorque(breakTorque);
                    return;
                }

                float speedLimit = speed >= 0 ? _data.MaxForwardSpeed : _data.MaxBackwardSpeed;
                
                if (LinearSpeed < speedLimit)
                {
                    _leftTrack.ApplyTorque(linearInput * wheelTorque);
                    _rightTrack.ApplyTorque(linearInput * wheelTorque);
                }
            }
        }

        private void HandleAngularMovement(Vector2 moveInputVector, float wheelTorque)
        {
            AngularSpeed = _tankRigidbody.angularVelocity.magnitude * Mathf.Rad2Deg;
            
            if (moveInputVector.x == 0) return;
            
            if (AngularSpeed > _data.RotationSpeed) return;
            
            UpdateGripFactor();

            float linearInput = moveInputVector.y;
            float angularInput = linearInput < 0 ? -moveInputVector.x : moveInputVector.x;

            if (linearInput != 0)
            {
                float breakTorque = _data.BrakeTorque * -linearInput;
                
                if (angularInput > 0)
                {
                    if (linearInput > 0)
                    {
                        _leftTrack.ApplyTorque(wheelTorque);
                        _rightTrack.ApplyTorque(breakTorque);
                    }
                    else if (linearInput < 0)
                    {
                        _leftTrack.ApplyTorque(breakTorque);
                        _rightTrack.ApplyTorque(-wheelTorque);
                    }
                }
                else if (angularInput < 0)
                {
                    if (linearInput > 0)
                    {
                        _leftTrack.ApplyTorque(breakTorque);
                        _rightTrack.ApplyTorque(wheelTorque);
                    }
                    else if (linearInput < 0)
                    {
                        _leftTrack.ApplyTorque(-wheelTorque);
                        _rightTrack.ApplyTorque(breakTorque);
                    }
                }
            }
            else
            {
                _leftTrack.ApplyTorque(wheelTorque * angularInput);
                _rightTrack.ApplyTorque(wheelTorque * -angularInput);
            }
        }
        
        private void UpdateGripFactor()
        {
            float maxSpeed = Mathf.Max(_data.MaxForwardSpeed, _data.MaxBackwardSpeed);
            if (maxSpeed <= 0) return;
            
            float speedRatio = LinearSpeed / maxSpeed;
            float gripFactor = _gripFactorCurve.Evaluate(speedRatio);
            
            _leftTrack.SetGripFactor(gripFactor);
            _rightTrack.SetGripFactor(gripFactor);
        }
        
        private float CalculateAvgWheelRpm()
        {
            float totalRpm = _leftTrack.CalculateAvgWheelRpm() + _rightTrack.CalculateAvgWheelRpm();
            int wheelsCount = _leftTrack.WheelsCount + _rightTrack.WheelsCount;

            if (wheelsCount == 0) return 0f;
            return totalRpm / wheelsCount;
        }

        private void InitializeColliders(TankHealth health)
        {
            if (!_trackColliderPrefab) return;
            
            Transform leftTrackCollider = 
                Instantiate(_trackColliderPrefab, _leftTrack.transform.position, _leftTrack.transform.rotation);
            
            Transform rightTrackCollider =
                Instantiate(_trackColliderPrefab, _rightTrack.transform.position, _rightTrack.transform.rotation);
            
            _collidersUpdater.AddCollider(leftTrackCollider, _leftTrack.transform);
            _collidersUpdater.AddCollider(rightTrackCollider, _rightTrack.transform);

            var leftTrackArmorAreas = leftTrackCollider.GetComponentsInChildren<Armor>();
            var rightTrackArmorAreas = rightTrackCollider.GetComponentsInChildren<Armor>();
            
            InitializeArmorAreas(leftTrackArmorAreas, health);
            InitializeArmorAreas(rightTrackArmorAreas, health);
            
            leftTrackCollider.parent = _collidersUpdater.transform;
            rightTrackCollider.parent = _collidersUpdater.transform;
            
            if (!_hullColliderPrefab) return;
            
            Transform hullCollider = Instantiate(_hullColliderPrefab, transform.position, transform.rotation);
            
            var hullArmorAreas = hullCollider.GetComponentsInChildren<Armor>();
            InitializeArmorAreas(hullArmorAreas, health);
            
            _collidersUpdater.AddCollider(hullCollider, transform);
            hullCollider.parent = _collidersUpdater.transform;
        }

        private static void InitializeArmorAreas(Armor[] armorAreas, TankHealth health)
        {
            foreach (Armor armor in armorAreas)
            {
                armor.Initialize(health);
            }
        }
    }
}