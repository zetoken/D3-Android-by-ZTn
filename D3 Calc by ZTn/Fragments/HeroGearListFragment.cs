using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
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

        public void updateHeroView(View view)
        {
            Console.WriteLine("HeroGearListFragment: updateHeroView");

            Hero hero = D3Context.getInstance().hero;
            if (hero != null)
            {
                ListView heroGearListView = view.FindViewById<ListView>(Resource.Id.heroGearListView);
                List<IListItem> gearAttr = new List<IListItem>()
                {
                    //new SectionHeaderListItem(Resources.GetString(Resource.String.Progress)),
                    //new AttributeListItem(Resources.GetString(Resource.String.level), hero.level),
                    //new AttributeListItem(Resources.GetString(Resource.String.paragon), hero.paragonLevel),
                };
                heroGearListView.Adapter = new SectionedListAdapter(Activity, gearAttr.ToArray());
            }
        }
    }
}