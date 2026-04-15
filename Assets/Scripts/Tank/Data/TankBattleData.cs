using System;

namespace Tank.Data
{
    public class TankBattleData
    {
        public string Id { get; private set; } 
        
        public TankBattleData()
        {
            GenerateId();
        }

        private void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}