using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace ZTnDroid.D3Calculator.Adapters.Delegated
{
    public class ListAdapter : BaseAdapter
    {
        Context context;
        IListItem[] items;
        public Boolean convertibleViews = false;

        #region >> Constructors

        public ListAdapter(Context context, IListItem[] attributes)
        {
            this.context = context;
            this.items = attributes;
        }

        #endregion

        #region >> BaseAdapter

        /// <inheritdoc/>
        public override int Count
        {
            get { return items.Length; }
        }

        /// <inheritdoc/>
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        /// <inheritdoc/>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <inheritdoc/>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;
            Boolean recycled = false;

            IListItem item = items[position];

            if (!convertibleViews || (convertView == null))
                view = ((Activity)context).LayoutInflater.Inflate(item.getLayoutResource(), parent, false);
            else
            {
                item.removeView(convertView);
                view = convertView;
                recycled = true;
            }

            item.updateView(view, recycled);

            return view;
        }

        /// <inheritdoc/>
        public override bool IsEnabled(int position)
        {
            return items[position].isEnabled();
        }

        #endregion
    }
}