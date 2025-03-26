using UnityEngine;

namespace VoxelObject
{
    public class HitDestructionInfo
    {
        public Vector3 HitPoint { get; }
        public float DamageRadius { get; }

        public HitDestructionInfo(Vector3 hitPoint, float damageRadius)
        {
            HitPoint = hitPoint;
            DamageRadius = damageRadius;
        }
    }
}