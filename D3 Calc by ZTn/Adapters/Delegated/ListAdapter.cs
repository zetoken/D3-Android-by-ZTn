using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System;

namespace ZTnDroid.D3Calculator.Adapters.Delegated
{
    public class ListAdapter : BaseAdapter
    {
        readonly Context context;
        readonly IListItem[] items;
        public Boolean ConvertibleViews = false;

        #region >> Constructors

        public ListAdapter(Context context, IListItem[] attributes)
        {
            this.context = context;
            items = attributes;
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

            if (!ConvertibleViews || (convertView == null))
                view = ((Activity)context).LayoutInflater.Inflate(item.GetLayoutResource(), parent, false);
            else
            {
                item.RemoveView(convertView);
                view = convertView;
                recycled = true;
            }

            item.UpdateView(view, recycled);

            return view;
        }

        /// <inheritdoc/>
        public override bool IsEnabled(int position)
        {
            return items[position].IsEnabled();
        }

        #endregion
    }
}