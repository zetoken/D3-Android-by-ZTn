using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3;
using ZTn.BNet.D3.Heroes;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Storage;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "View Hero", Theme = "@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class ViewHeroActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("ViewHeroActivity: OnCreate");
            base.OnCreate(bundle);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Fragment fragment = new HeroCharacteristicsListFragment();
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.fragment_container, fragment);
            fragmentTransaction.Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}