using AI;
using UnityEngine;

namespace Input
{
    public class AIInput : MonoBehaviour, IInput
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _gunPivot;
        [SerializeField] private LayerMask _targetMask = 1 << 7;
        [SerializeField] private LayerMask _fovMask = 1 << 10;
        [SerializeField] private float _stopDistance = 50f;
        
        private const float RotationThreshold = 0.95f;
        private const float MaxAimDistance = 100f;
        
        private Pathfinder _pathfinder;
        private Transform _targetCenter;

        public void SetPathfinder(Pathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public void SetTarget(Transform target)
        {
            _targetCenter = target;
        }

        public Vector2 GetMoveInput()
        {
            float distance = Vector3.Distance(_center.position, _targetCenter.position);

            if (IsTargetInFOV() && distance <= _stopDistance)
            {
                return Vector2.zero;
            }

            Vector2 moveInputVector = Vector2.zero;
            _pathfinder.FindPath(_center.position, _targetCenter.position);

            if (_pathfinder.TryGetNextPathCell(out var cell))
            {
                Vector3 targetDirection = (cell.WorldPosition - _center.position).normalized;
                targetDirection.y = 0;
                float dot = Vector3.Dot(_center.forward, targetDirection);
                
                if (dot < RotationThreshold)
                {
                    if (RotationThreshold - dot > 1 - RotationThreshold)
                    {
                        moveInputVector.y = 0;
                    }
                    
                    Vector3 cross = Vector3.Cross(_center.forward, targetDirection);
                    
                    if (cross.y > 0) moveInputVector.x = 1;
                    else moveInputVector.x = -1;
                }
                else
                {
                    moveInputVector.x = 0;
                    
                    if (Vector3.Distance(_center.position, cell.WorldPosition) > 0.1f)
                    {
                        moveInputVector.y = 1;
                    }
                    else
                    {
                        moveInputVector.y = 0;
                    }
                }
            }
            
            return moveInputVector;
        }

        public Vector3 GetLookInput()
        {
            return _targetCenter.position;
        }

        public bool GetShootInput()
        {
            // TODO: Add aiming prediction and spread
            Ray ray = new Ray(_gunPivot.position, _gunPivot.forward);
            return Physics.Raycast(ray, MaxAimDistance, _targetMask.value);
        }

        private bool IsTargetInFOV()
        {
            Vector3 direction = _targetCenter.position - _center.position;
            Ray ray = new Ray(_center.position, direction);
            return !Physics.Raycast(ray, direction.magnitude, _fovMask.value);
        }
    }
}