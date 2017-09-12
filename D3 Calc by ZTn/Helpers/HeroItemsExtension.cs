using System.Reflection;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Helpers
{
    static class HeroItemsExtension
    {
        private static ItemSummary UpdateToFullItem(ItemSummary item)
        {
            if (item == null)
            {
                return item;
            }

            return item.GetFullItem();
        }

        /// <summary>
        /// Fetches full <see cref="Item"/>s of all <see cref="ItemSummary"/>s.
        /// </summary>
        /// <param name="heroItems"></param>
        public static void UpdateToFullItems(this HeroItems heroItems)
        {
            heroItems.Head = UpdateToFullItem(heroItems.Head);
            heroItems.Torso = UpdateToFullItem(heroItems.Torso);
            heroItems.Feet = UpdateToFullItem(heroItems.Feet);
            heroItems.Hands = UpdateToFullItem(heroItems.Hands);
            heroItems.Shoulders = UpdateToFullItem(heroItems.Shoulders);
            heroItems.Legs = UpdateToFullItem(heroItems.Legs);
            heroItems.Bracers = UpdateToFullItem(heroItems.Bracers);
            heroItems.Waist = UpdateToFullItem(heroItems.Waist);
            heroItems.RightFinger = UpdateToFullItem(heroItems.RightFinger);
            heroItems.LeftFinger = UpdateToFullItem(heroItems.LeftFinger);
            heroItems.Neck = UpdateToFullItem(heroItems.Neck);
            heroItems.MainHand = UpdateToFullItem(heroItems.MainHand);
            heroItems.OffHand = UpdateToFullItem(heroItems.OffHand);

            heroItems.MainHand = heroItems.MainHand ?? ZTn.BNet.D3.Calculator.Helpers.Constants.NakedHandWeapon;
            heroItems.OffHand = heroItems.OffHand ?? ZTn.BNet.D3.Calculator.Helpers.Constants.BlankWeapon;
        }
    }
}