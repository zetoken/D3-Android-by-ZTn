using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/SettingsActivityLabel")]
    public class SettingsActivity : ZTnFragmentActivity
    {
        #region >> ZTnFragmentActivity

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            base.OnCreate(savedInstanceState);

            this.Application.SetTheme(Android.Resource.Style.ThemeHolo);

            SetContentView(Resource.Layout.FragmentContainer);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SupportFragmentManager
                .BeginTransaction()
                .Add(Resource.Id.fragment_container, new SettingsFragment())
                .Commit();
        }

        #endregion
    }
}