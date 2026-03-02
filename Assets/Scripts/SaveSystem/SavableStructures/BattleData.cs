using System;

namespace SaveSystem.SavableStructures
{
    [Serializable]
    public class BattleData
    {
        public int TankId { get; private set; }

        public BattleData(int tankId)
        {
            TankId = tankId;
        }
    }
}