using System.Collections;
using System.Collections.Generic;
using ArmorSystem;
using Navigation;
using Tank;
using UnityEngine;
using VContainer;
using UtilityAI;
using Screen = ArmorSystem.Screen;

namespace InputSystem
{
    public class AIInput : MonoBehaviour, IInput
    {
        [SerializeField] private List<AIAction> _availableActions;
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _gunPivot;
        [SerializeField] private LayerMask _targetMask = 1 << 7;
        [SerializeField] private LayerMask _fovMask = 1 << 10;
        [SerializeField] private float _pathUpdateInterval = 1f;
        [SerializeField] private int _scanGridSize = 3;
        [SerializeField] private float _scanRadius = 1.5f;

        private TankController _tankController;
        private Pathfinder _pathfinder;
        private IRouterTargetRegistry _routerTargetRegistry;
        private Router _router;
        
        private List<NavGridCell> _path;
        private int _currentPathIndex;
        private Coroutine _updatePathCoroutine;

        private AIAction _bestAction;
        private AIContext _currentContext;

        public NavGridCell TargetPathCell { get; private set; }
        public bool IsEnabled { get; private set; }
        
        [Inject]
        public void Construct(Pathfinder pathfinder, IRouterTargetRegistry routerTargetRegistry)
        {
            _pathfinder = pathfinder;
            _routerTargetRegistry = routerTargetRegistry;
        }
        
        public void Initialize()
        {
            Enable();
            
            if (!TryGetComponent(out _tankController)) return;
            
            var targets = FilterRouterTargets();
            _router = new Router(targets);
            _updatePathCoroutine = StartCoroutine(UpdatePathWithInterval());
        }

        public void Destroy() { }

        public void Enable()
        {
            IsEnabled = true;
        } 
        
        public void Disable()
        {
            IsEnabled = false;
            if (_updatePathCoroutine != null) StopCoroutine(_updatePathCoroutine);
        }

        private void Update()
        {
            if (!IsEnabled || _router == null || _router.CurrentTarget == null) return;

            _currentContext = PopulateContext();
            _bestAction = SelectBestAction(_currentContext);
        }
        
        private AIContext PopulateContext()
        {
            var targetTank = _router.CurrentTarget as TankController;
            float distance = targetTank ? Vector3.Distance(_center.position, targetTank.transform.position) : 0f;

            Vector3 bestAimPoint = targetTank ? GetWeakestVisibleArmorPoint(targetTank) : _center.position + _center.forward;

            return new AIContext
            {
                Position = _center.position,
                Self = _tankController,
                Target = targetTank,
                DistanceToTarget = distance,
                HasLineOfSight = IsTargetInFOV(targetTank),
                IsAimingAtTarget = CheckDirectLineOfSight(bestAimPoint), // Проверяем прицел именно на эту точку
                HasValidPath = _path is { Count: > 0 },
                CurrentHealthNormalized = 1.0f,
                
                BestAimPoint = bestAimPoint // Передаем точку в контекст
            };
        }

        private Vector3 GetWeakestVisibleArmorPoint(TankController targetTank)
        {
            Vector3 targetCenter = targetTank.transform.position;
            Vector3 directionToTarget = (targetCenter - _gunPivot.position).normalized;

            Vector3 rightSpace = Vector3.Cross(Vector3.up, directionToTarget).normalized;
            Vector3 upSpace = Vector3.Cross(directionToTarget, rightSpace).normalized;

            Vector3 bestPoint = targetCenter;
            float minEffectiveArmor = float.MaxValue;

            for (int ix = 0; ix < _scanGridSize; ix++)
            {
                for (int iy = 0; iy < _scanGridSize; iy++)
                {
                    float tX = (float)ix / (_scanGridSize - 1);
                    float tY = (float)iy / (_scanGridSize - 1);

                    // Переводим диапазон [0, 1] в диапазон [-1, 1] для симметрии вокруг центра
                    float normX = (tX * 2f) - 1f;
                    float normY = (tY * 2f) - 1f;

                    // Вычисляем итоговое смещение луча
                    float offsetX = normX * _scanRadius;
                    float offsetY = normY * _scanRadius;

                    Vector3 rayTargetPoint = targetCenter + (rightSpace * offsetX) + (upSpace * offsetY);
                    Vector3 rayDirection = rayTargetPoint - _gunPivot.position;
                    Debug.DrawRay(_gunPivot.position, rayDirection, Color.red);
                    
                    if (Physics.Raycast(_gunPivot.position, rayDirection.normalized, out RaycastHit hit, 20, _targetMask.value))
                    {
                        if (!hit.collider.TryGetComponent<Armor>(out var armor)) continue;

                        if (armor is Screen) continue;
                        
                        float hitAngle = Vector3.Angle(rayDirection, -hit.normal);
                        float armorThickness = armor.GetReducedThickness(hitAngle);

                        if (armorThickness < minEffectiveArmor)
                        {
                            minEffectiveArmor = armorThickness;
                            bestPoint = hit.point;
                        }
                    }
                }
            }

            return bestPoint;
        }

        private AIAction SelectBestAction(AIContext context)
        {
            AIAction highestAction = null;
            float maxScore = -1f;

            foreach (var action in _availableActions)
            {
                float score = action.EvaluateUtility(context);
                if (score > maxScore)
                {
                    maxScore = score;
                    highestAction = action;
                }
            }

            return highestAction;
        }

        #region Интерфейс IInput

        public Vector2 GetMoveInput()
        {
            if (!IsEnabled || !_bestAction) return Vector2.zero;
            
            return _bestAction.ProcessMovement(this, _currentContext);
        }

        public Vector3 GetLookPoint()
        {
            if (_currentContext.Target) return _currentContext.Target.transform.position;
            return _center.position + _center.forward;
        }

        public bool GetShootInput()
        {
            if (!IsEnabled || !_bestAction) return false;
            return _bestAction.ProcessShooting(this, _currentContext);
        }

        #endregion

        #region Публичные хелперы для AIAction

        public float GetRotationInputTo(Vector3 targetPosition)
        {
            Vector3 targetDirection = (targetPosition - _center.position).normalized;
            Vector3 localDirection = transform.InverseTransformDirection(targetDirection);
            return Mathf.Clamp(localDirection.x, -1, 1);
        }

        public void AdvancePathIndex()
        {
            if (_path == null || _currentPathIndex >= _path.Count)
            {
                TargetPathCell = null;
                return;
            }
            TargetPathCell = _path[_currentPathIndex];
            _currentPathIndex++;
        }

        #endregion

        #region Внутренняя логика (Проверки физики и Пути)

        private bool IsTargetInFOV(TankController targetTank)
        {
            if (!targetTank) return false;
            
            Vector3 direction = targetTank.transform.position - _center.position;
            Ray ray = new Ray(_center.position, direction);
            return !Physics.Raycast(ray, direction.magnitude, _fovMask.value);
        }

        private bool CheckDirectLineOfSight()
        {
            Ray ray = new Ray(_gunPivot.position, _gunPivot.forward);
            return Physics.Raycast(ray, 20, _targetMask.value);
        }
        
        private bool CheckDirectLineOfSight(Vector3 targetPoint)
        {
            Vector3 direction = targetPoint - _gunPivot.position;
            Ray ray = new Ray(_gunPivot.position, direction.normalized);
            return Physics.Raycast(ray, 20, _targetMask.value);
        }

        private void UpdatePath()
        {
            if (!_pathfinder || _router == null) return;
            _router.UpdateTarget(_center.position);
            
            if (_router.CurrentTarget is MonoBehaviour target)
            {
                _path = _pathfinder.FindPath(_center.position, target.transform.position);
                _currentPathIndex = 0;
                TargetPathCell = null; 
                AdvancePathIndex();
            }
        }

        private IEnumerator UpdatePathWithInterval()
        {
            if (!_pathfinder) yield break;
            while (true)
            {
                UpdatePath();
                yield return new WaitForSeconds(_pathUpdateInterval);
            }
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

        #endregion
    }
}
