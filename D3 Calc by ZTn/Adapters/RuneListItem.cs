using System;

using Android.Views;
using Android.Widget;

using ZTn.BNet.D3.Skills;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class RuneListItem : IListItem
    {
        public Rune rune;

        #region >> Constructors

        public RuneListItem(Rune rune)
        {
            this.rune = rune;
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int getLayoutResource()
        {
            return Resource.Layout.RuneListItem;
        }

        /// <inheritdoc/>
        public bool isEnabled()
        {
            return false;
        }

        /// <inheritdoc/>
        public void removeView(View view)
        {
        }

        /// <inheritdoc/>
        public void updateView(View view, Boolean recycled)
        {
            view.FindViewById<TextView>(Resource.Id.runeName).Text = rune.name;
            view.FindViewById<TextView>(Resource.Id.runeDescription).Text = rune.description;
            //view.FindViewById<ImageView>(Resource.Id.imageSkill)
        }

        #endregion
    }
}
