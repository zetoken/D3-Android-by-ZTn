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
            String description = "";
            if (item.attributes != null)
            {
                foreach (String s in item.attributes)
                    description += s + System.Environment.NewLine;
            }
            view.FindViewById<TextView>(Resource.Id.gearItemDescription).Text = description;
            switch (item.displayColor)
            {
                case "orange":
                    view.FindViewById<TextView>(Resource.Id.gearItemName).SetTextColor(view.Resources.GetColor(Resource.Color.orangeItem));
                    break;
                case "yellow":
                    view.FindViewById<TextView>(Resource.Id.gearItemName).SetTextColor(view.Resources.GetColor(Resource.Color.yellowItem));
                    break;
                case "green":
                    view.FindViewById<TextView>(Resource.Id.gearItemName).SetTextColor(view.Resources.GetColor(Resource.Color.greenItem));
                    break;
                case "blue":
                    view.FindViewById<TextView>(Resource.Id.gearItemName).SetTextColor(view.Resources.GetColor(Resource.Color.blueItem));
                    break;
                default:
                    break;
            }
            //view.FindViewById<ImageView>(Resource.Id.imageGearItem).SetImageBitmap(...);
        }
    }
}
