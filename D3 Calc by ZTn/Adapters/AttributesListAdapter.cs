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
    public class AttributesListAdapter : BaseAdapter
    {
        Context context;
        AttributeDescriptor[] attributes;

        public AttributesListAdapter(Context context, AttributeDescriptor[] attributes)
        {
            this.context = context;
            this.attributes = attributes;
        }

        public override int Count
        {
            get { return attributes.Length; }
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
            if (convertView == null)
            {
                view = ((Activity)context).LayoutInflater.Inflate(Resource.Layout.AttributeListItem, parent, false);
            }
            else
            {
                view = convertView;
            }

            AttributeDescriptor attribute = attributes[position];
            view.FindViewById<TextView>(Resource.Id.attributeName).Text = attribute.name;
            view.FindViewById<TextView>(Resource.Id.attributeValue).Text = attribute.value;

            return view;
        }

        public AttributeDescriptor getAttributeAt(int position)
        {
            return attributes[position];
        }
    }
}