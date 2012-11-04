using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Fragments;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "Settings", Theme = "@android:style/Theme.Holo", Icon = "@drawable/icon")]
    public class SettingsActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("SettingsActivity: OnCreate");
            base.OnCreate(bundle);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Fragment fragment = new SettingsFragment();
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