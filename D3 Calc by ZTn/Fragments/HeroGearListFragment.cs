using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator.Fragments
{
    public class HeroGearListFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("HeroGearListFragment: OnCreate");
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Console.WriteLine("HeroGearListFragment: OnCreateView");
            View view = inflater.Inflate(Resource.Layout.ViewHeroGear, container, false);

            updateHeroView(view);

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

        private void updateHeroView(View view)
        {
            Console.WriteLine("HeroGearListFragment: updateHeroView");

            Hero hero = D3Context.getInstance().hero;
            if (hero != null && hero.items != null)
            {
                ListView heroGearListView = view.FindViewById<ListView>(Resource.Id.heroGearListView);
                List<IListItem> gearAttr = new List<IListItem>();

                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemHead), hero.items.head, D3Context.getInstance().headIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemTorso), hero.items.torso, D3Context.getInstance().torsoIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemFeet), hero.items.feet, D3Context.getInstance().feetIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemHands), hero.items.hands, D3Context.getInstance().handsIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemShoulders), hero.items.shoulders, D3Context.getInstance().shouldersIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemLegs), hero.items.legs, D3Context.getInstance().legsIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemBracers), hero.items.bracers, D3Context.getInstance().bracersIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemMainHand), hero.items.mainHand, D3Context.getInstance().mainHandIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemOffHand), hero.items.offHand, D3Context.getInstance().offHandIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemWaist), hero.items.waist, D3Context.getInstance().waistIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemRightFinger), hero.items.rightFinger, D3Context.getInstance().rightFingerIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemLeftFinger), hero.items.leftFinger, D3Context.getInstance().leftFingerIcon));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemNeck), hero.items.neck, D3Context.getInstance().neckIcon));

                heroGearListView.Adapter = new SectionedListAdapter(Activity, gearAttr.ToArray());
            }
        }
    }
}