using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Tank;
using UnityEngine;

namespace Input
{
    public class AIInput : Input
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
        private Router _router;
        private NavGridCell _targetPathCell;
        private Transform _playerTankCenter;

        public void Initialize(Pathfinder pathfinder, List<IRouterTarget> routerTargets,
            IRouterTarget defaultRouterTarget)
        {
            _pathfinder = pathfinder;
            _router = new Router(routerTargets, defaultRouterTarget);

            if (defaultRouterTarget is not PlayerTank playerTank)
            {
                throw new Exception("The default router target must be a PlayerTank");
            }
            
            _playerTankCenter = playerTank.Center;
            StartCoroutine(UpdatePath());
        }

        private void OnEnable()
        {
            Pathfinder.OnPathRetraced += UpdateTargetPathCell;
        }

        private void OnDisable()
        {
            Pathfinder.OnPathRetraced -= UpdateTargetPathCell;
        }

        public override Vector2 GetMoveInput()
        {
            if (!IsActive) return Vector2.zero;
            
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

        public override bool GetShootInput()
        {
            if (!IsActive) return false;
            
            // TODO: Add aiming prediction and spread
            Ray ray = new Ray(_gunPivot.position, _gunPivot.forward);
            return Physics.Raycast(ray, MaxAimDistance, _targetMask.value);
        }

        private IEnumerator UpdatePath()
        {
            if (!_pathfinder) yield break;
            
            // TODO: Make end of cycle when tank destroyed
            while (true)
            {
                _router.UpdateTarget(_center.position);
                MonoBehaviour target = _router.CurrentTarget as MonoBehaviour;

                if (target)
                {
                    _pathfinder.FindPath(_center.position, target.transform.position);
                }
                
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