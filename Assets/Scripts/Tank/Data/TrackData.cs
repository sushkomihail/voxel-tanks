using System;
using UnityEngine;

namespace Tank.Data
{
    [Serializable]
    public class TrackData : TankModuleData
    {
        [SerializeField] private float _damagedTorqueRate = 0.5f;
        
        public float DamagedTorqueRate => _damagedTorqueRate;
    }
}