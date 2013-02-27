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
    [Activity(Label = "@string/SettingsActivityLabel")]
    public class SettingsActivity : ZTnFragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("SettingsActivity: OnCreate");
            base.OnCreate(savedInstanceState);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SupportFragmentManager
                .BeginTransaction()
                .Add(Resource.Id.fragment_container, new SettingsFragment())
                .Commit();
        }
    }
}