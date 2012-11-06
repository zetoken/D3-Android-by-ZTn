using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class GearItemListItem : IListItem
    {
        public Item item;

        public GearItemListItem(Item item)
        {
            this.item = item;
        }

        public int getLayoutResource()
        {
            return Resource.Layout.GearItemListItem;
        }

        public void updateHeroView(View view)
        {
            view.FindViewById<TextView>(Resource.Id.gearItemName).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.gearItemDescription).Text = item.id;
            //view.FindViewById<ImageView>(Resource.Id.imageGearItem).SetImageBitmap(...);
        }
    }
}
