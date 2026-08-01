using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class Router
    {
        public IRouterTarget CurrentTarget { get; private set; }

        private readonly Dictionary<IRouterTarget, float> _sqrDistances = new();

        public Router(List<IRouterTarget> targets)
        {
            if (targets is { Count: > 0 })
            {
                CurrentTarget = targets[0];
                
                foreach (IRouterTarget target in targets)
                {
                    _sqrDistances.Add(target, 0f);
                }
            }
        }

        public void UpdateTarget(Vector3 from)
        {
            if (_sqrDistances.Count == 0) return;
            
            IRouterTarget closestTarget = null;
            float minSqrDistance = float.MaxValue;
            
            foreach (var pair in _sqrDistances)
            {
                IRouterTarget target = pair.Key;

                if (target is not { IsActive: true }) continue;
                
                float sqrDistance = (from - target.Position).sqrMagnitude;

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestTarget = target;
                }
            }

            if (closestTarget != null)
            {
                CurrentTarget = closestTarget;
            }
            else if (CurrentTarget is { IsActive: false })
            {
                CurrentTarget = null; 
            }
        }
    }
}