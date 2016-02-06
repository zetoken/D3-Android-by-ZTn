using ZTn.BNet.D3.Heroes;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class HeroData : IControlData
    {
        public HeroSummary Hero { get; set; }

        public HeroData(HeroSummary hero)
        {
            Hero = hero;
        }
    }
}
