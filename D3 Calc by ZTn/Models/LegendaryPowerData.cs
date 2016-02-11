using Xamarin.Forms;
using ZTn.BNet.D3.Heroes;
using ZTn.Pcl.D3Calculator.Resources;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class LegendaryPowerData : IControlData
    {
        public LegendaryPower LegendaryPower { get; }
        public KanaiCubePosition Position { get; }

        public Color Color
        {
            get
            {
                switch (LegendaryPower.DisplayColor)
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
            get { return ImageSource.FromFile("icon.png"); }
        }

        public LegendaryPowerData(LegendaryPower legendaryPowerLegendaryPower, KanaiCubePosition position)
        {
            Position = position;
            LegendaryPower = legendaryPowerLegendaryPower;
        }
    }
}
