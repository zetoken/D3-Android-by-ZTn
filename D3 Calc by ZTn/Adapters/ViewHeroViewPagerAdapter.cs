using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using ZTnDroid.D3Calculator.Helpers;

using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ZTnDroid.D3Calculator.Adapters
{
    class ViewHeroViewPagerAdapter : FragmentPagerAdapter
    {
        List<Fragment> fragments;
        List<String> titles;

        public ViewHeroViewPagerAdapter(Context context, FragmentManager fragmentManager, List<Fragment> fragments, List<String> titles)
            : base(fragmentManager)
        {
            this.fragments = fragments;
            this.titles = titles;
        }

        public override int Count
        {
            get { return fragments.Count; }
        }

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position].ToUpper());
        }
    }
}