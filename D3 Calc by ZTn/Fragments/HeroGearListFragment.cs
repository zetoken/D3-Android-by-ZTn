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
            if (item == null)
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
                GetDataForItem(Resource.String.itemHead, heroItems.head as Item, icons.Head),
                GetDataForItem(Resource.String.itemTorso, heroItems.torso as Item, icons.Torso),
                GetDataForItem(Resource.String.itemFeet, heroItems.feet as Item, icons.Feet),
                GetDataForItem(Resource.String.itemHands, heroItems.hands as Item, icons.Hands),
                GetDataForItem(Resource.String.itemShoulders, heroItems.shoulders as Item, icons.Shoulders),
                GetDataForItem(Resource.String.itemLegs, heroItems.legs as Item, icons.Legs),
                GetDataForItem(Resource.String.itemBracers, heroItems.bracers as Item, icons.Bracers),
                GetDataForItem(Resource.String.itemMainHand, heroItems.mainHand as Item, icons.MainHand),
                GetDataForItem(Resource.String.itemOffHand, heroItems.offHand as Item, icons.OffHand),
                GetDataForItem(Resource.String.itemWaist, heroItems.waist as Item, icons.Waist),
                GetDataForItem(Resource.String.itemRightFinger, heroItems.rightFinger as Item, icons.RightFinger),
                GetDataForItem(Resource.String.itemLeftFinger, heroItems.leftFinger as Item, icons.LeftFinger),
                GetDataForItem(Resource.String.itemNeck, heroItems.neck as Item, icons.Neck)
            };

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