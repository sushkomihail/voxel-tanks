using Tank.Modules;

namespace EquipmentSystem
{
    public class RepairKitFactory : EquipmentItemFactory
    {
        private readonly TankModule[] _tankModules;
        
        public RepairKitFactory(TankModule[] tankModules)
        {
            _tankModules = tankModules;
        }
        
        public override EquipmentItem CreateItem()
        {
            return new RepairKit(_tankModules);
        }
    }
}