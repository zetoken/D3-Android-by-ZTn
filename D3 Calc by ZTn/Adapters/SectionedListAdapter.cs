using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ZTn.BNet.D3.Heroes;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SectionedListAdapter : BaseAdapter
    {
        Context context;
        IListItem[] items;

        public SectionedListAdapter(Context context, IListItem[] attributes)
        {
            this.context = context;
            this.items = attributes;
        }

        public override int Count
        {
            get { return items.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view;

            IListItem item = items[position];

            //if (convertView == null)
            view = ((Activity)context).LayoutInflater.Inflate(item.getLayoutResource(), parent, false);
            //else
            //    view = convertView;

            item.updateHeroView(view);

            return view;
        }

        public IListItem getAttributeAt(int position)
        {
            return items[position];
        }
    }
}