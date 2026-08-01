using InputSystem;
using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(fileName = "MoveAlongPathAction", menuName = "UtilityAI/Actions/Move Along Path")]
    public class MoveAlongPathAction : AIAction
    {
        protected const float RotationThreshold = 0.5f;
        protected const float MoveThreshold = 3f;

        public override Vector2 ProcessMovement(AIInput ai, AIContext context)
        {
            if (ai.TargetPathCell == null) return Vector2.zero;

            Vector2 moveInput = Vector2.zero;
            moveInput.x = ai.GetRotationInputTo(ai.TargetPathCell.WorldPosition);

            if (Mathf.Abs(moveInput.x) < RotationThreshold)
            {
                if (Vector3.Distance(context.Position, ai.TargetPathCell.WorldPosition) > MoveThreshold)
                {
                    moveInput.y = 1f;
                }
                else
                {
                    ai.AdvancePathIndex();
                }
            }
            return moveInput;
        }

        public override bool ProcessShooting(AIInput ai, AIContext context) => false;
    }
}