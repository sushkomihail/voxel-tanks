using System.Collections.Generic;
using EquipmentSystem;
using UnityEngine;
using Vehicles;

namespace Tank.Data
{
    [CreateAssetMenu(fileName = "_Data", menuName = "Tank Data")]
    public class TankData : ScriptableObject
    {
        [SerializeField] private ChassisData _chassisData;
        [SerializeField] private EngineData _engineData;
        [SerializeField] private TransmissionData _transmissionData;
        [SerializeField] private TrackData _trackData;
        [SerializeField] private TurretData _turretData;
        [SerializeField] private GunData _gunData;
        [SerializeField] private HealthData _healthData;
        [SerializeField] private EquipmentItemType[] _equipment;
        
        public ChassisData ChassisData => _chassisData;
        public EngineData EngineData => _engineData;
        public TransmissionData TransmissionData => _transmissionData;
        public TrackData TrackData => _trackData;
        public TurretData TurretData => _turretData;
        public GunData GunData => _gunData;
        public HealthData HealthData => _healthData;
        public IReadOnlyList<EquipmentItemType> Equipment => _equipment;
    }
}