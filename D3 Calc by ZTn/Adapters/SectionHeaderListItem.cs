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

namespace ZTnDroid.D3Calculator.Adapters
{
    public class SectionHeaderListItem : IListItem
    {
        public String label;

        public SectionHeaderListItem(String label)
            : base()
        {
            this.label = label;
        }

        public int getLayoutResource()
        {
            return Resource.Layout.SectionHeaderListItem;
        }

        public void updateInView(View view)
        {
            view.FindViewById<TextView>(Resource.Id.sectionLabel).Text = label;
        }
    }
}