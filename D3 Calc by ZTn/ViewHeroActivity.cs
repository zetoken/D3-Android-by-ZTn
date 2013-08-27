using System;
using System.Collections.Generic;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.View;
using Android.Widget;
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
        bool forceRefresh = false;

        FetchHeroFragment fragmentFetchHero;

        #region >> Fragment

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            switch (requestCode)
            {
                case GearItemEditorActivity.ITEM_EDIT:
                    switch (resultCode)
                    {
                        case Result.Ok:
                            Toast.MakeText(this, Resources.GetString(Resource.String.ItemEditingFinished), ToastLength.Long).Show();
                            forceRefresh = true;
                            break;

                        case Result.Canceled:
                            Toast.MakeText(this, Resources.GetString(Resource.String.ItemEditingCanceled), ToastLength.Long).Show();
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            // Activity initialization
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SimpleViewPager);

            this.Title = D3Context.instance.heroSummary.name;
            this.ActionBar.Subtitle = D3Context.instance.battleTag;

            // ViewPager initialization
            List<Fragment> fragments = new List<Fragment>();
            List<String> titles = new List<string>();

            if (savedInstanceState == null)
            {
                fragmentFetchHero = new FetchHeroFragment();
            }
            else
            {
                fragmentFetchHero = (FetchHeroFragment)SupportFragmentManager.FindFragmentByTag("fetchHero");
            }

            fragments.Add(fragmentFetchHero.fragmentCharacteristics);
            titles.Add(Resources.GetString(Resource.String.details));

            fragments.Add(fragmentFetchHero.fragmentComputed);
            titles.Add(Resources.GetString(Resource.String.computed));

            fragments.Add(fragmentFetchHero.fragmentSkills);
            titles.Add(Resources.GetString(Resource.String.skills));

            fragments.Add(fragmentFetchHero.fragmentGear);
            titles.Add(Resources.GetString(Resource.String.gear));

            pagerAdapter = new Adapters.BasicViewPagerAdapter(SupportFragmentManager, fragments, titles);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.Pager);
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
            ZTnTrace.trace(MethodInfo.GetCurrentMethod());

            if (forceRefresh)
            {
                fragmentFetchHero.fragmentComputed.updateFragment();
                fragmentFetchHero.fragmentSkills.updateFragment();
                fragmentFetchHero.fragmentGear.updateFragment();
                forceRefresh = false;
            }

            base.OnResume();
        }

        #endregion
    }
}