using System.Reflection;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Helpers
{
    static class LegendaryPowerExtension
    {
        private static void UpdateToFullItem(ref LegendaryPower power)
        {
            if (power == null)
            {
                return;
            }

            power = power.GetFullItem();
        }

        /// <summary>
        /// Fetches full <see cref="Item"/>s of all <see cref="ItemSummary"/>s.
        /// </summary>
        /// <param name="powers"></param>
        public static void UpdateToFullItems(this LegendaryPower[] powers)
        {
            if (powers != null)
            {
                foreach (var power in powers)
                {
                    UpdateToFullItem(power);
                }
            }
        }
    }
}