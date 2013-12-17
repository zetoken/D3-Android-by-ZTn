using Android.App;
using Android.Views;
using Android.Widget;
using System;
using ZTnDroid.D3Calculator.Adapters.Delegated;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SectionHeaderListItem : IListItem
    {
        public String Label;

        #region >> Constructors

        public SectionHeaderListItem(int id)
            : this(Application.Context.Resources.GetString(id))
        {
        }

        public SectionHeaderListItem(String label)
        {
            Label = label.ToUpper();
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int GetLayoutResource()
        {
            return Resource.Layout.SectionHeaderListItem;
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
            view.FindViewById<TextView>(Resource.Id.sectionLabel).Text = Label;
        }

        #endregion
    }
}