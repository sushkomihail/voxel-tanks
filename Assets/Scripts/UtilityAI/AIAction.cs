using InputSystem;
using UnityEngine;

namespace UtilityAI
{
    public abstract class AIAction : ScriptableObject
    {
        [SerializeField] private string actionName;
        [SerializeField] private AIConsideration[] considerations;

        public float EvaluateUtility(AIContext context)
        {
            if (considerations == null || considerations.Length == 0) return 0f;

            float totalScore = 1f;

            foreach (var consideration in considerations)
            {
                float score = consideration.Score(context);
                totalScore *= score; 

                if (totalScore <= 0f) return 0f;
            }

            return totalScore;
        }

        public abstract Vector2 ProcessMovement(AIInput ai, AIContext context);
        public abstract bool ProcessShooting(AIInput ai, AIContext context);
    }
}