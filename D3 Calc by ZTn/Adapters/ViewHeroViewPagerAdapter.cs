using System;
using System.Collections.Generic;
using Android.Support.V4.App;

using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace ZTnDroid.D3Calculator.Adapters
{
    class BasicViewPagerAdapter : FragmentPagerAdapter
    {
        List<Fragment> fragments;
        List<String> titles;

        #region >> Constructors

        public BasicViewPagerAdapter(FragmentManager fragmentManager, List<Fragment> fragments, List<String> titles)
            : base(fragmentManager)
        {
            this.fragments = fragments;
            this.titles = titles;
        }

        #endregion

        #region >> FragmentPagerAdapter

        /// <inheritdoc/>
        public override int Count
        {
            get { return fragments.Count; }
        }

        /// <inheritdoc/>
        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        /// <inheritdoc/>
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position].ToUpper());
        }

        #endregion
    }
}