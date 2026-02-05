namespace Tank.Modules.Engine
{
    public class CriticalEngineState : EngineState
    {
        private const float CriticalTorqueRate = 0;
        
        public CriticalEngineState(Engine engine) : base(engine)
        {
        }

        public override void Enter()
        {
            _engine.SetTorqueRate(CriticalTorqueRate);
        }
    }
}