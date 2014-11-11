using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.OS;
using Android.Views;
using Android.Widget;
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

        private IListItem GetDataForItem(int id, Item itemSummary, D3Picture icon)
        {
            return GetDataForItem(Resources.GetString(id), itemSummary, icon);
        }

        private static IListItem GetDataForItem(String label, Item item, D3Picture icon)
        {
            if (item == null || item.Name == null)
            {
                return null;
            }

            if (icon != null)
            {
                return new GearItemListItem(label, item, icon);
            }

            return new GearItemListItem(label, item);
        }

        private void UpdateView(View view)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            var heroItems = D3Context.Instance.CurrentHeroItems;
            var icons = D3Context.Instance.Icons;

            if (heroItems == null)
            {
                return;
            }

            var heroGearListView = view.FindViewById<ListView>(Resource.Id.heroGearListView);
            var gearListItems = new List<IListItem>
            {
                GetDataForItem(Resource.String.itemHead, heroItems.Head as Item, icons.Head),
                GetDataForItem(Resource.String.itemTorso, heroItems.Torso as Item, icons.Torso),
                GetDataForItem(Resource.String.itemFeet, heroItems.Feet as Item, icons.Feet),
                GetDataForItem(Resource.String.itemHands, heroItems.Hands as Item, icons.Hands),
                GetDataForItem(Resource.String.itemShoulders, heroItems.Shoulders as Item, icons.Shoulders),
                GetDataForItem(Resource.String.itemLegs, heroItems.Legs as Item, icons.Legs),
                GetDataForItem(Resource.String.itemBracers, heroItems.Bracers as Item, icons.Bracers),
                GetDataForItem(Resource.String.itemMainHand, heroItems.MainHand as Item, icons.MainHand),
                GetDataForItem(Resource.String.itemOffHand, heroItems.OffHand as Item, icons.OffHand),
                GetDataForItem(Resource.String.itemWaist, heroItems.Waist as Item, icons.Waist),
                GetDataForItem(Resource.String.itemRightFinger, heroItems.RightFinger as Item, icons.RightFinger),
                GetDataForItem(Resource.String.itemLeftFinger, heroItems.LeftFinger as Item, icons.LeftFinger),
                GetDataForItem(Resource.String.itemNeck, heroItems.Neck as Item, icons.Neck)
            };

            var items = new List<Item>
            {
                (Item)heroItems.Bracers,
                (Item)heroItems.Feet,
                (Item)heroItems.Hands,
                (Item)heroItems.Head,
                (Item)heroItems.LeftFinger,
                (Item)heroItems.Legs,
                (Item)heroItems.Neck,
                (Item)heroItems.RightFinger,
                (Item)heroItems.Shoulders,
                (Item)heroItems.Torso,
                (Item)heroItems.Waist,
                (Item)heroItems.MainHand,
                (Item)heroItems.OffHand
            };
            items = items
                .Where(i => i != null)
                .ToList();

            var setItemDiscount = items.Aggregate(ItemValueRange.Zero, (current, item) => current + item.AttributesRaw.AttributeSetItemDiscount);

            foreach (var set in D3Context.Instance.ActivatedSets)
            {
                var setItemCount = set.CountItemsOfSet(items);

                if (setItemCount >= 2)
                {
                    setItemCount += (int)setItemDiscount.Min;
                }

                foreach (var rank in set.ranks.Where(r => r.Required <= setItemCount))
                {
                    var setItem = new Item
                    {
                        Name = string.Format("{0} ({1})", set.name, rank.Required),
                        Attributes = rank.Attributes,
                        DisplayColor = "green"
                    };
                    gearListItems.Add(GetDataForItem(Resource.String.setBonuses, setItem, null));
                }
            }

            heroGearListView.Adapter = new ListAdapter(Activity, gearListItems
                .Where(l => l != null)
                .ToArray()) { ConvertibleViews = true };
        }
    }
}