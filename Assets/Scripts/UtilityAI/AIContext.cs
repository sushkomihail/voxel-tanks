using UnityEngine;
using Tank;

namespace UtilityAI
{
    public struct AIContext
    {
        public Vector3 Position;
        public TankController Self;
        public TankController Target;
        public bool HasLineOfSight;
        public bool IsAimingAtTarget;
        public float DistanceToTarget;
        public bool HasValidPath;
        public float CurrentHealthNormalized;
        public Vector3 BestAimPoint;
    }
}