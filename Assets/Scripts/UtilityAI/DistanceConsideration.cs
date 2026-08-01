using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(fileName = "DistanceConsideration", menuName = "UtilityAI/Considerations/Distance")]
    public class DistanceConsideration : AIConsideration
    {
        [SerializeField] private float maxDistance = 50f;

        protected override float GetRawValue(AIContext context)
        {
            if (!context.Target) return 0f;
            return context.DistanceToTarget / maxDistance;
        }
    }
}