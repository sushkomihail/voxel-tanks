using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(fileName = "FOVConsideration", menuName = "UtilityAI/Considerations/FOV")]
    public class FOVConsideration : AIConsideration
    {
        protected override float GetRawValue(AIContext context)
        {
            return context.HasLineOfSight ? 1f : 0f;
        }
    }
}