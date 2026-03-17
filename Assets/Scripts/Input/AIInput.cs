using System.Collections;
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
        [SerializeField] private float _stopDistance = 10f;
        [SerializeField] private float _pathUpdateInterval = 1f;
        
        private const float RotationThreshold = 0.5f;
        private const float MoveThreshold = 3f;
        private const float MaxAimDistance = 100f;
        
        private Pathfinder _pathfinder;
        private NavGridCell _targetPathCell;
        private Transform _playerTankCenter;

        private void OnEnable()
        {
            Pathfinder.OnPathRetraced += UpdateTargetPathCell;
        }

        private void OnDisable()
        {
            Pathfinder.OnPathRetraced -= UpdateTargetPathCell;
        }

        public void SetPathfinder(Pathfinder pathfinder)
        {
            _pathfinder = pathfinder;
        }

        public void SetPlayerTankCenter(Transform center)
        {
            _playerTankCenter = center;
        }

        public Vector2 GetMoveInput()
        {
            float distanceToPlayer = Vector3.Distance(_center.position, _playerTankCenter.position);
            Vector2 moveInputVector = Vector2.zero;
            
            if (IsTargetInFOV()) {
                moveInputVector.x = GetRotationInput(_playerTankCenter.position);
            }
            
            if (distanceToPlayer <= _stopDistance)
            {
                return moveInputVector;
            }
            
            if (_targetPathCell == null) return Vector2.zero;
            
            moveInputVector.x = GetRotationInput(_targetPathCell.WorldPosition);
            
            if (Mathf.Abs(moveInputVector.x) < RotationThreshold)
            {
                if (Vector3.Distance(_center.position, _targetPathCell.WorldPosition) > MoveThreshold)
                {
                    moveInputVector.y = 1;
                }
                else
                {
                    moveInputVector.y = 0;
                    UpdateTargetPathCell();
                }
            }
            
            return moveInputVector;
        }

        public Vector3 GetLookInput()
        {
            return _playerTankCenter.position;
        }

        public bool GetShootInput()
        {
            // TODO: Add aiming prediction and spread
            Ray ray = new Ray(_gunPivot.position, _gunPivot.forward);
            return Physics.Raycast(ray, MaxAimDistance, _targetMask.value);
        }

        public IEnumerator UpdatePath()
        {
            if (!_pathfinder) yield break;
            
            // TODO: Make end of cycle
            while (true)
            {
                _pathfinder.FindPath(_center.position, _playerTankCenter.position);
                yield return new WaitForSeconds(_pathUpdateInterval);
            }
        }

        private bool IsTargetInFOV()
        {
            Vector3 direction = _playerTankCenter.position - _center.position;
            Ray ray = new Ray(_center.position, direction);
            return !Physics.Raycast(ray, direction.magnitude, _fovMask.value);
        }

        private void UpdateTargetPathCell()
        {
            if (_pathfinder.TryGetNextPathCell(out NavGridCell cell))
            {
                _targetPathCell = cell;
            }
        }

        private float GetRotationInput(Vector3 observedPosition)
        {
            Vector3 targetDirection = (observedPosition - _center.position).normalized;
            Vector3 localDirection = transform.InverseTransformDirection(targetDirection);
            return Mathf.Clamp(localDirection.x, -1, 1);
        }
    }
}