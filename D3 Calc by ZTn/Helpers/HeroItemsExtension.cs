using System.Reflection;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Helpers
{
    static class HeroItemsExtension
    {
        private static void UpdateToFullItem(ref ItemSummary item)
        {
            if (item == null)
            {
                return;
            }

            item = item.GetFullItem();
        }

        /// <summary>
        /// Fetches full <see cref="Item"/>s of all <see cref="ItemSummary"/>s.
        /// </summary>
        /// <param name="heroItems"></param>
        public static void UpdateToFullItems(this HeroItems heroItems)
        {
            UpdateToFullItem(ref heroItems.Head);
            UpdateToFullItem(ref heroItems.Torso);
            UpdateToFullItem(ref heroItems.Feet);
            UpdateToFullItem(ref heroItems.Hands);
            UpdateToFullItem(ref heroItems.Shoulders);
            UpdateToFullItem(ref heroItems.Legs);
            UpdateToFullItem(ref heroItems.Bracers);
            UpdateToFullItem(ref heroItems.Waist);
            UpdateToFullItem(ref heroItems.RightFinger);
            UpdateToFullItem(ref heroItems.LeftFinger);
            UpdateToFullItem(ref heroItems.Neck);
            UpdateToFullItem(ref heroItems.MainHand);
            UpdateToFullItem(ref heroItems.OffHand);

            heroItems.MainHand = heroItems.MainHand ?? ZTn.BNet.D3.Calculator.D3Calculator.NakedHandWeapon;
            heroItems.OffHand = heroItems.OffHand ?? ZTn.BNet.D3.Calculator.D3Calculator.BlankWeapon;
        }
    }
}