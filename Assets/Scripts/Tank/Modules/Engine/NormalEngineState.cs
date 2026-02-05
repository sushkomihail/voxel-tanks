namespace Tank.Modules.Engine
{
    public class NormalEngineState : EngineState
    {
        private const float NormalTorqueRate = 1;
        
        public NormalEngineState(Engine engine) : base(engine)
        {
        }

        public override void Enter()
        {
            _engine.SetTorqueRate(NormalTorqueRate);
        }
    }
}