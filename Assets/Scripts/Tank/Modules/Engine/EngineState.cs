namespace Tank.Modules.Engine
{
    public abstract class EngineState
    {
        protected readonly Engine _engine;
        
        protected EngineState(Engine engine)
        {
            _engine = engine;
        }
        
        public abstract void Enter();
    }
}