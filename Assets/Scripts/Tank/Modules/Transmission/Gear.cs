namespace Tank.Modules.Transmission
{
    public class Gear
    {
        public float SpeedLimit { get; private set; }
        public bool IsReverse { get; private set; }

        public Gear(float speedLimit)
        {
            SpeedLimit = speedLimit;
        }
        
        public Gear(float speedLimit, bool isReverse)
        {
            SpeedLimit = speedLimit;
            IsReverse = isReverse;
        }
    }
}