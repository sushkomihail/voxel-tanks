using System.Collections;
using System.Collections.Generic;
using Environment.Base;
using Navigation;
using Tank;
using UnityEngine;
using VContainer;

namespace InputSystem
{
    public class NPCInput : Input
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

        private TankController _tankController;
        private Pathfinder _pathfinder;
        private IRouterTargetRegistry _routerTargetRegistry;
        private Router _router;
        private NavGridCell _targetPathCell;
        private Coroutine _updatePathCoroutine;
        
        [Inject]
        public void Construct(Pathfinder pathfinder, IRouterTargetRegistry routerTargetRegistry)
        {
            _pathfinder = pathfinder;
            _routerTargetRegistry = routerTargetRegistry;
        }
        
        public override void Initialize()
        {
            if (TryGetComponent(out _tankController))
            {
                var targets = FilterRouterTargets();
                _router = new Router(targets);
                _updatePathCoroutine = StartCoroutine(UpdatePathWithInterval());
            }
        }

        private void OnEnable()
        {
            Pathfinder.OnPathRetraced += UpdateTargetPathCell;
            CapturedState.OnCaptured += UpdatePath;
        }

        private void OnDisable()
        {
            Pathfinder.OnPathRetraced -= UpdateTargetPathCell;
            CapturedState.OnCaptured -= UpdatePath;
        }

        public override void Disable()
        {
            base.Disable();
            
            StopCoroutine(_updatePathCoroutine);
            _pathfinder.ClearPath();
        }

        public override Vector2 GetMoveInput()
        {
            if (!IsActive) return Vector2.zero;
            
            float distanceToPlayer = Vector3.Distance(_center.position, ((TankController)_router.CurrentTarget).transform.position);
            Vector2 moveInputVector = Vector2.zero;
            
            if (IsTargetInFOV()) {
                moveInputVector.x = GetRotationInput(((TankController)_router.CurrentTarget).transform.position);
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
            return ((TankController)_router.CurrentTarget).transform.position;
        }

        public override bool GetShootInput()
        {
            if (!IsActive) return false;
            
            Ray ray = new Ray(_gunPivot.position, _gunPivot.forward);
            return Physics.Raycast(ray, MaxAimDistance, _targetMask.value);
        }

        private List<IRouterTarget> FilterRouterTargets()
        {
            var targets = new List<IRouterTarget>();

            foreach (IRouterTarget target in _routerTargetRegistry.Targets)
            {
                if (target is TankController tankController && tankController == _tankController) continue;
                
                targets.Add(target);
            }
            
            return targets;
        }

        private IEnumerator UpdatePathWithInterval()
        {
            if (!_pathfinder) yield break;
            
            // TODO: Make end of cycle when tank destroyed
            while (true)
            {
                UpdatePath();
                yield return new WaitForSeconds(_pathUpdateInterval);
            }
        }

        private void UpdatePath()
        {
            if (!_pathfinder || _router == null) return;
            
            _router.UpdateTarget(_center.position);
            MonoBehaviour target = _router.CurrentTarget as MonoBehaviour;

            if (target)
            {
                _pathfinder.FindPath(_center.position, target.transform.position);
            }
        }

        private bool IsTargetInFOV()
        {
            Vector3 direction = ((TankController)_router.CurrentTarget).transform.position - _center.position;
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