namespace EquipmentSystem
{
    public class RepairKitFactory : EquipmentItemFactory
    {
        public override IEquipmentItem CreateItem()
        {
            return new RepairKit();
        }
    }
}