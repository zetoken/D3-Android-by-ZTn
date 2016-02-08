using Xamarin.Forms;
using ZTn.BNet.D3.Items;
using ZTn.Pcl.D3Calculator.Resources;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class ItemSummaryData : IControlData
    {
        public ItemSummary Item { get; }
        public ItemPosition Position { get; }

        public Color Color
        {
            get
            {
                switch (Item.DisplayColor)
                {
                    case "orange":
                        return Colors.Legendary;
                    case "blue":
                        return Colors.Magic;
                    case "green":
                        return Colors.Set;
                    case "yellow":
                        return Colors.Rare;
                    case "white":
                        return Colors.Trash;
                    case "gray":
                        return Colors.Normal;
                    default:
                        return Colors.Normal;
                }
            }
        }

        public ImageSource Icon
        {
            get { return ImageSource.FromFile("icon.png");}
        }

        public ItemSummaryData(ItemSummary itemItem, ItemPosition position)
        {
            Position = position;
            Item = itemItem;
        }
    }
}
