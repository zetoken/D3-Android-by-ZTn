using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroGearListFragment : ZTnFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroGearListFragment: OnCreate");
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroGearListFragment: OnCreateView");
            View view = inflater.Inflate(Resource.Layout.ViewHeroGear, container, false);

            updateView(view);

            return view;
        }

        private List<IListItem> getPartialViewForItem(String label, ItemSummary item, D3Picture icon)
        {
            List<IListItem> list = new List<IListItem>() {
                new SectionHeaderListItem(label)
            };

            if (item != null && (item is Item) && (icon != null))
                list.Add(new GearItemListItem((Item)item, icon));
            else if (item != null && (item is Item))
                list.Add(new GearItemListItem((Item)item));

            return list;
        }

        private void updateView(View view)
        {
            Console.WriteLine("HeroGearListFragment: updateHeroView");

            Hero hero = D3Context.getInstance().hero;
            IconsContainer icons = D3Context.getInstance().icons;
            if (hero != null && hero.items != null)
            {
                ListView heroGearListView = view.FindViewById<ListView>(Resource.Id.heroGearListView);
                List<IListItem> gearAttr = new List<IListItem>();

                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemHead), hero.items.head, icons.head));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemTorso), hero.items.torso, icons.torso));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemFeet), hero.items.feet, icons.feet));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemHands), hero.items.hands, icons.hands));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemShoulders), hero.items.shoulders, icons.shoulders));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemLegs), hero.items.legs, icons.legs));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemBracers), hero.items.bracers, icons.bracers));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemMainHand), hero.items.mainHand, icons.mainHand));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemOffHand), hero.items.offHand, icons.offHand));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemWaist), hero.items.waist, icons.waist));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemRightFinger), hero.items.rightFinger, icons.rightFinger));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemLeftFinger), hero.items.leftFinger, icons.leftFinger));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemNeck), hero.items.neck, icons.neck));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.setBonuses), D3Context.getInstance().setBonus, null));

                heroGearListView.Adapter = new SectionedListAdapter(Activity, gearAttr.ToArray());
            }
        }
    }
}