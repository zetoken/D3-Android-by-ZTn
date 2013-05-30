using System;

using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SectionHeaderListItem : IListItem
    {
        public String label;

        #region >> Constructors

        public SectionHeaderListItem(int id)
            : this(D3Calc.Context.Resources.GetString(id))
        {
        }

        public SectionHeaderListItem(String label)
            : base()
        {
            this.label = label.ToUpper();
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int getLayoutResource()
        {
            return Resource.Layout.SectionHeaderListItem;
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
            view.FindViewById<TextView>(Resource.Id.sectionLabel).Text = label;
        }

        #endregion
    }
}