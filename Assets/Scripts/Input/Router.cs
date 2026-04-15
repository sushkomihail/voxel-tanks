using System.Collections.Generic;
using System.Linq;
using Environment.Base;
using UnityEngine;

namespace Input
{
    public class Router
    {
        public IRouterTarget CurrentTarget { get; private set; }

        private readonly Dictionary<IRouterTarget, float> _sqrDistances = new();

        public Router(List<IRouterTarget> targets, IRouterTarget defaultTarget)
        {
            CurrentTarget = defaultTarget;
            
            foreach (IRouterTarget target in targets)
            {
                _sqrDistances.Add(target, 0f);
                
                if (target is Tank.Tank tankModel)
                {
                    tankModel.Health.OnDeath += () => _sqrDistances.Remove(target);
                }
            }
        }

        public void UpdateTarget(Vector3 from)
        {
            if (_sqrDistances.Count == 0) return;
            
            IRouterTarget closestTarget = null;
            float minSqrDistance = _sqrDistances[CurrentTarget];
            
            foreach (IRouterTarget target in _sqrDistances.Keys.ToList())
            {
                if (target is not MonoBehaviour behaviour) continue;
                
                float sqrDistance = (from - behaviour.transform.position).sqrMagnitude;

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestTarget = target;
                }
                
                _sqrDistances[target] = sqrDistance;
            }

            if (CurrentTarget is BaseModel baseReference && 
                baseReference.IsPositionInside(from) && 
                baseReference.IsCapturing)
            {
                return;
            }

            if (closestTarget != null)
            {
                CurrentTarget = closestTarget;
            }
        }
    }
}