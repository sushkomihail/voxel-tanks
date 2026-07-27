using System;

namespace EquipmentSystem
{
    public abstract class EquipmentItem
    {
        public event Action OnUsed;
        
        public abstract EquipmentItemType Type { get; }
        public int Count { get; protected set; }

        public void UpdateCount(int count)
        {
            Count = count;
        }

        public virtual bool TryUse()
        {
            if (Count == 0) return false;
            return true;
        }
    }
}