using AI;
using UnityEngine;

namespace Tank.AI
{
    public class AIInput : MonoBehaviour
    {
        [SerializeField] private Transform _gunPivot;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private float _stopDistance = 50f;
        
        private const float RotationThreshold = 0.95f;
        
        private Pathfinder _pathfinder;
        private Vector2 _moveInputVector;
        
        public Transform Target { get; private set; }
        public Vector2 MoveInputVector => _moveInputVector;

        public void SetPathfinder(Pathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }

        public void Process()
        {
            float distance = Vector3.Distance(transform.position, Target.position);

            if (IsTargetInFOV() && distance <= _stopDistance)
            {
                _moveInputVector = Vector2.zero;
                return;
            }
            
            _pathfinder.FindPath(transform.position, Target.position);

            if (_pathfinder.TryGetNextPathCell(out var cell))
            {
                Vector3 targetDirection = (cell.WorldPosition - transform.position).normalized;
                targetDirection.y = 0;
                float dot = Vector3.Dot(transform.forward, targetDirection);
                
                if (dot < RotationThreshold)
                {
                    if (RotationThreshold - dot > 1 - RotationThreshold)
                    {
                        _moveInputVector.y = 0;
                    }
                    
                    Vector3 cross = Vector3.Cross(transform.forward, targetDirection);
                    
                    if (cross.y > 0) _moveInputVector.x = 1;
                    else _moveInputVector.x = -1;
                }
                else
                {
                    _moveInputVector.x = 0;
                    
                    if (Vector3.Distance(transform.position, cell.WorldPosition) > 0.1f)
                    {
                        _moveInputVector.y = 1;
                    }
                    else
                    {
                        _moveInputVector.y = 0;
                    }
                }
            } 
        }

        public bool IsGunAimedToTarget()
        {
            // TODO: Add aiming prediction
            return Physics.Raycast(_gunPivot.position, _gunPivot.forward, out RaycastHit _);
        }

        private bool IsTargetInFOV()
        {
            Vector3 direction = Target.position - transform.position;
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, _targetMask))
            {
                return hit.transform.root == Target.root;
            }
            
            return false;
        }
    }
}