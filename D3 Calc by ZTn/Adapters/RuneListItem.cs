using Android.Views;
using Android.Widget;
using System;
using ZTn.BNet.D3.Skills;
using ZTnDroid.D3Calculator.Adapters.Delegated;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class RuneListItem : IListItem
    {
        public Rune Rune;

        #region >> Constructors

        public RuneListItem(Rune rune)
        {
            Rune = rune;
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int GetLayoutResource()
        {
            return Resource.Layout.RuneListItem;
        }

        /// <inheritdoc/>
        public bool IsEnabled()
        {
            return false;
        }

        /// <inheritdoc/>
        public void RemoveView(View view)
        {
        }

        /// <inheritdoc/>
        public void UpdateView(View view, Boolean recycled)
        {
            view.FindViewById<TextView>(Resource.Id.runeName).Text = Rune.name;
            view.FindViewById<TextView>(Resource.Id.runeDescription).Text = Rune.description;
            //view.FindViewById<ImageView>(Resource.Id.imageSkill)
        }

        #endregion
    }
}
