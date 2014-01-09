using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZTn.BNet.D3.Calculator.Sets;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Adapters.Delegated;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroGearListFragment : UpdatableFragment
    {
        #region >> UpdatableFragment

        /// <inheritdoc/>
        public override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        /// <inheritdoc/>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var view = inflater.Inflate(Resource.Layout.ViewHeroGear, container, false);

            UpdateView(view);

            return view;
        }

        #endregion

        private IListItem GetDataForItem(int id, ItemSummary itemSummary, D3Picture icon)
        {
            return GetDataForItem(Resources.GetString(id), itemSummary, icon);
        }

        private static IListItem GetDataForItem(String label, ItemSummary itemSummary, D3Picture icon)
        {
            var item = itemSummary as Item;

            if (item == null)
            {
                return null;
            }

            if (icon != null)
            {
                return new GearItemListItem(label, (Item)itemSummary, icon);
            }

            return new GearItemListItem(label, (Item)itemSummary);
        }

        private void UpdateView(View view)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var hero = D3Context.Instance.CurrentHero;
            var icons = D3Context.Instance.Icons;
            if (hero != null && hero.items != null)
            {
                var heroGearListView = view.FindViewById<ListView>(Resource.Id.heroGearListView);
                var gearAttr = new List<IListItem>
                {
                    GetDataForItem(Resource.String.itemHead, hero.items.head, icons.Head),
                    GetDataForItem(Resource.String.itemTorso, hero.items.torso, icons.Torso),
                    GetDataForItem(Resource.String.itemFeet, hero.items.feet, icons.Feet),
                    GetDataForItem(Resource.String.itemHands, hero.items.hands, icons.Hands),
                    GetDataForItem(Resource.String.itemShoulders, hero.items.shoulders, icons.Shoulders),
                    GetDataForItem(Resource.String.itemLegs, hero.items.legs, icons.Legs),
                    GetDataForItem(Resource.String.itemBracers, hero.items.bracers, icons.Bracers),
                    GetDataForItem(Resource.String.itemMainHand, hero.items.mainHand, icons.MainHand),
                    GetDataForItem(Resource.String.itemOffHand, hero.items.offHand, icons.OffHand),
                    GetDataForItem(Resource.String.itemWaist, hero.items.waist, icons.Waist),
                    GetDataForItem(Resource.String.itemRightFinger, hero.items.rightFinger, icons.RightFinger),
                    GetDataForItem(Resource.String.itemLeftFinger, hero.items.leftFinger, icons.LeftFinger),
                    GetDataForItem(Resource.String.itemNeck, hero.items.neck, icons.Neck)
                };

                var heroItems = D3Context.Instance.CurrentHero.items;
                var items = new List<Item>
                {
                    (Item)heroItems.bracers,
                    (Item)heroItems.feet,
                    (Item)heroItems.hands,
                    (Item)heroItems.head,
                    (Item)heroItems.leftFinger,
                    (Item)heroItems.legs,
                    (Item)heroItems.neck,
                    (Item)heroItems.rightFinger,
                    (Item)heroItems.shoulders,
                    (Item)heroItems.torso,
                    (Item)heroItems.waist,
                    (Item)heroItems.mainHand,
                    (Item)heroItems.offHand
                };
                items = items.Where(i => i != null).ToList();

                foreach (var set in D3Context.Instance.ActivatedSets)
                {
                    var setItem = new Item { name = set.name, attributes = set.getBonusAttributes(set.countItemsOfSet(items)), displayColor = "green" };
                    if (setItem.attributes.Length > 0)
                        gearAttr.Add(GetDataForItem(Resource.String.setBonuses, setItem, null));
                }

                heroGearListView.Adapter = new ListAdapter(Activity, gearAttr.Where(l => l != null).ToArray()) { ConvertibleViews = true };
            }
        }
    }
}