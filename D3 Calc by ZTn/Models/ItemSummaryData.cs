using ZTn.BNet.D3.Items;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class ItemSummaryData : IControlData
    {
        public ItemSummary Item { get; }
        public ItemPosition Position { get; }

        public ItemSummaryData(ItemSummary itemItem, ItemPosition position)
        {
            Position = position;
            Item = itemItem;
        }
    }
}
