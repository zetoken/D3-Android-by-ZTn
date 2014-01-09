using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.View;
using Android.Widget;
using System.Collections.Generic;
using System.Reflection;
using ZTnDroid.D3Calculator.Adapters;
using ZTnDroid.D3Calculator.Fragments;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;
using Fragment = Android.Support.V4.App.Fragment;

namespace ZTnDroid.D3Calculator
{
    [Activity(Label = "@string/ViewHeroActivityLabel")]
    public class ViewHeroActivity : ZTnFragmentActivity
    {
        PagerAdapter pagerAdapter;
        bool forceRefresh;

        FetchHeroFragment fragmentFetchHero;

        public ViewHeroActivity()
        {
            forceRefresh = false;
        }

        #region >> Fragment

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            switch (requestCode)
            {
                case GearItemEditorActivity.ItemEdit:
                    switch (resultCode)
                    {
                        case Result.Ok:
                            Toast.MakeText(this, Resources.GetString(Resource.String.ItemEditingFinished), ToastLength.Long).Show();
                            forceRefresh = true;
                            break;

                        case Result.Canceled:
                            Toast.MakeText(this, Resources.GetString(Resource.String.ItemEditingCanceled), ToastLength.Long).Show();
                            break;
                    }
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            // Activity initialization
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SimpleViewPager);

            Title = D3Context.Instance.CurrentHeroSummary.name;
            ActionBar.Subtitle = D3Context.Instance.BattleTag;

            // ViewPager initialization
            var fragments = new List<Fragment>();
            var titles = new List<string>();

            if (savedInstanceState == null)
            {
                fragmentFetchHero = new FetchHeroFragment();
            }
            else
            {
                fragmentFetchHero = (FetchHeroFragment)SupportFragmentManager.FindFragmentByTag("fetchHero");
            }

            fragments.Add(fragmentFetchHero.FragmentCharacteristics);
            titles.Add(Resources.GetString(Resource.String.details));

            fragments.Add(fragmentFetchHero.FragmentComputed);
            titles.Add(Resources.GetString(Resource.String.computed));

            fragments.Add(fragmentFetchHero.FragmentSkills);
            titles.Add(Resources.GetString(Resource.String.skills));

            fragments.Add(fragmentFetchHero.FragmentGear);
            titles.Add(Resources.GetString(Resource.String.gear));

            pagerAdapter = new BasicViewPagerAdapter(SupportFragmentManager, fragments, titles);

            var viewPager = FindViewById<ViewPager>(Resource.Id.Pager);
            viewPager.Adapter = pagerAdapter;

            // Fragment in charge of fetching hero initialization
            if (savedInstanceState == null)
            {
                SupportFragmentManager
                    .BeginTransaction()
                    .Add(fragmentFetchHero, "fetchHero")
                    .Commit();
            }
        }

        protected override void OnResume()
        {
            ZTnTrace.Trace(MethodBase.GetCurrentMethod());

            if (forceRefresh)
            {
                fragmentFetchHero.FragmentComputed.UpdateFragment();
                fragmentFetchHero.FragmentSkills.UpdateFragment();
                fragmentFetchHero.FragmentGear.UpdateFragment();
                forceRefresh = false;
            }

            base.OnResume();
        }

        #endregion
    }
}