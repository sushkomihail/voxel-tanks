using AI;
using UnityEngine;

namespace Tank.AI
{
    public class AIInput : MonoBehaviour
    {
        [SerializeField] private Pathfinder _pathfinder;
        [SerializeField] private Transform _target;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private float _stopDistance = 50f;
        
        private const float RotationThreshold = 0.95f;
        
        private Vector2 _moveInputVector;
        
        public Transform Target => _target;
        public Vector2 MoveInputVector => _moveInputVector;

        public void SetPathfinder(Pathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Process()
        {
            float distance = Vector3.Distance(transform.position, _target.position);

            if (IsTargetInFOV() && distance <= _stopDistance)
            {
                _moveInputVector = Vector2.zero;
                return;
            }
            
            _pathfinder.FindPath(transform.position, _target.position);

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

        private bool IsTargetInFOV()
        {
            Vector3 direction = _target.position - transform.position;
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, _targetMask))
            {
                return hit.transform.root == _target.root;
            }
            
            return false;
        }
    }
}