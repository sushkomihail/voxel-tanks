using System.Collections.Generic;

namespace Navigation
{
    public interface IRouterTargetRegistry
    {
        public IReadOnlyList<IRouterTarget> Targets { get; }
        public void Register(IRouterTarget target);
        public void Unregister(IRouterTarget target);
    }
}