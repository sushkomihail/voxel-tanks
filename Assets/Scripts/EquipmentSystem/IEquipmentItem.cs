namespace EquipmentSystem
{
    public interface IEquipmentItem
    {
        public EquipmentItemType Type { get; }
        public void Use();
    }
}