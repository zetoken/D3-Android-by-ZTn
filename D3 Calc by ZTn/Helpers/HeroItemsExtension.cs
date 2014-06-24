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
            UpdateToFullItem(ref heroItems.head);
            UpdateToFullItem(ref heroItems.torso);
            UpdateToFullItem(ref heroItems.feet);
            UpdateToFullItem(ref heroItems.hands);
            UpdateToFullItem(ref heroItems.shoulders);
            UpdateToFullItem(ref heroItems.legs);
            UpdateToFullItem(ref heroItems.bracers);
            UpdateToFullItem(ref heroItems.waist);
            UpdateToFullItem(ref heroItems.rightFinger);
            UpdateToFullItem(ref heroItems.leftFinger);
            UpdateToFullItem(ref heroItems.neck);

            heroItems.mainHand = heroItems.mainHand != null ? heroItems.mainHand.GetFullItem() : ZTn.BNet.D3.Calculator.D3Calculator.NakedHandWeapon;
            heroItems.offHand = heroItems.offHand != null ? heroItems.offHand.GetFullItem() : ZTn.BNet.D3.Calculator.D3Calculator.BlankWeapon;
        }
    }
}