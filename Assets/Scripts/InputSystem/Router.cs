using System.Collections.Generic;
using System.Linq;
using Environment.Base;
using Tank;
using UnityEngine;

namespace InputSystem
{
    public class Router
    {
        public IRouterTarget CurrentTarget { get; private set; }

        private readonly Dictionary<IRouterTarget, float> _sqrDistances = new();
        private readonly Dictionary<IRouterTarget, bool> _priorities = new();
        private readonly IRouterTarget _defaultTarget;

        public Router(List<IRouterTarget> targets, IRouterTarget defaultTarget)
        {
            _defaultTarget = defaultTarget;
            CurrentTarget = defaultTarget;
            
            foreach (IRouterTarget target in targets)
            {
                _sqrDistances.Add(target, 0f);

                if (target is BaseModel)
                {
                    _priorities.Add(target, true);
                }
                
                if (target is TankController tankModel)
                {
                    tankModel.Health.OnDeath += () => _sqrDistances.Remove(target);
                }
            }
        }

        public void UpdateTarget(Vector3 from)
        {
            if (_sqrDistances.Count == 0) return;
            
            IRouterTarget closestTarget = null;
            float minSqrDistance = _sqrDistances[_defaultTarget];
            
            foreach (IRouterTarget target in _sqrDistances.Keys.ToList())
            {
                if (target is not MonoBehaviour behaviour) continue;
                
                if (_priorities.ContainsKey(target) && !_priorities[target]) continue;
                
                float sqrDistance = (from - behaviour.transform.position).sqrMagnitude;

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestTarget = target;
                }
                
                _sqrDistances[target] = sqrDistance;
            }

            if (CurrentTarget is BaseModel baseModel && 
                baseModel.IsPositionInside(from) && 
                (baseModel.IsCapturing || baseModel.IsRecapturing))
            {
                return;
            }

            if (closestTarget != null)
            {
                CurrentTarget = closestTarget;
            }
        }

        public void UpdatePriorities(string tankId)
        {
            foreach (IRouterTarget target in _priorities.Keys.ToList())
            {
                var baseModel = (BaseModel)target;
                _priorities[baseModel] = tankId != baseModel.OwnerId;
            }
        }
    }
}