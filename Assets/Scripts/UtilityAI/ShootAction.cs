using InputSystem;
using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(fileName = "ShootAction", menuName = "UtilityAI/Actions/Shoot")]
    public class ShootAction : AIAction
    {
        public override Vector2 ProcessMovement(AIInput ai, AIContext context)
        {
            if (!context.Target) return Vector2.zero;
            return new Vector2(ai.GetRotationInputTo(context.Target.transform.position), 0f);
        }

        public override bool ProcessShooting(AIInput ai, AIContext context)
        {
            return context.IsAimingAtTarget;
        }
    }
}