using ZTn.BNet.D3.Items;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class ItemData : IControlData
    {
        public Item Item { get; }
        public ItemPosition Position { get; }

        public ItemData(Item itemItem, ItemPosition position)
        {
            Position = position;
            Item = itemItem;
        }
    }
}
