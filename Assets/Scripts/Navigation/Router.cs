using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    public class Router
    {
        public IRouterTarget CurrentTarget { get; private set; }

        private readonly Dictionary<IRouterTarget, float> _sqrDistances = new();

        public Router(List<IRouterTarget> targets)
        {
            CurrentTarget = targets[0];
            
            foreach (IRouterTarget target in targets)
            {
                _sqrDistances.Add(target, 0f);
            }
        }

        public void UpdateTarget(Vector3 from)
        {
            if (_sqrDistances.Count == 0) return;
            
            IRouterTarget closestTarget = null;
            float minSqrDistance = _sqrDistances[CurrentTarget];
            
            foreach (IRouterTarget target in _sqrDistances.Keys.ToList())
            {
                if (!target.IsActive) continue;
                
                float sqrDistance = (from - target.Position).sqrMagnitude;

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestTarget = target;
                }
                
                _sqrDistances[target] = sqrDistance;
            }

            if (closestTarget != null)
            {
                CurrentTarget = closestTarget;
            }
        }
    }
}