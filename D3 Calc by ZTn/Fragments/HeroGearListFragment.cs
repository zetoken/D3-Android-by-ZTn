using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
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

        private List<IListItem> getPartialViewForItem(String label, ItemSummary item)
        {
            List<IListItem> list = new List<IListItem>() {
                new SectionHeaderListItem(label)
            };

            if (item != null && (item is Item))
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

                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemHead), hero.items.head));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemTorso), hero.items.torso));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemFeet), hero.items.feet));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemHands), hero.items.hands));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemShoulders), hero.items.shoulders));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemLegs), hero.items.legs));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemBracers), hero.items.bracers));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemMainHand), hero.items.mainHand));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemOffHand), hero.items.offHand));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemWaist), hero.items.waist));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemRightFinger), hero.items.rightFinger));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemLeftFinger), hero.items.leftFinger));
                gearAttr.AddRange(getPartialViewForItem(Resources.GetString(Resource.String.itemNeck), hero.items.neck));

                heroGearListView.Adapter = new SectionedListAdapter(Activity, gearAttr.ToArray());
            }
        }
    }
}