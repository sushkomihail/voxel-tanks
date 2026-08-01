using UnityEngine;

namespace UtilityAI
{
    public abstract class AIConsideration : ScriptableObject
    {
        [SerializeField] protected AnimationCurve responseCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float Score(AIContext context)
        {
            float rawValue = GetRawValue(context);
            float clampedValue = Mathf.Clamp01(rawValue);
            return responseCurve.Evaluate(clampedValue);
        }

        protected abstract float GetRawValue(AIContext context);
    }
}