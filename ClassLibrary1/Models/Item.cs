using System;

namespace ClassLibrary1.Models
{
    public class Item : IEquatable<Item>
    {
        public string ItemId;
        public ItemTypeEnum ItemType;
        
        public bool Equals(Item item)
        {
            if (item is null)
                return false;

            return ItemId.Equals(item.ItemId);
        }
        
        public override bool Equals(object obj) => Equals(obj as Item);
        public override int GetHashCode() => (ItemId, ItemType).GetHashCode();
    }
}