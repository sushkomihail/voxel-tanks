namespace Tank.Modules.Engine
{
    public class DamagedEngineState : EngineState
    {
        public DamagedEngineState(Engine engine) : base(engine)
        {
        }

        public override void Enter()
        {
            _engine.SetTorqueRate(_engine.DamagedTorqueRate);
        }
    }
}