using System.Collections.Generic;

namespace Navigation
{
    public class RouterTargetRegistry : IRouterTargetRegistry
    {
        private readonly List<IRouterTarget>  _targets = new();
        
        public IReadOnlyList<IRouterTarget> Targets => _targets;

        public void Register(IRouterTarget target)
        {
            _targets.Add(target);
        }

        public void Unregister(IRouterTarget target)
        {
            _targets.Remove(target);
        }
    }
}