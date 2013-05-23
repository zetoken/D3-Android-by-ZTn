using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class GearItemListItem : IListItem
    {
        public String label;
        public Item item;
        public D3Picture icon;

        public GearItemListItem(String label, Item item)
        {
            this.label = label;
            this.item = item;
        }

        public GearItemListItem(String label, Item item, D3Picture icon)
        {
            this.label = label;
            this.item = item;
            this.icon = icon;
        }

        public int getLayoutResource()
        {
            return Resource.Layout.GearItemListItem;
        }

        public void updateHeroView(View view)
        {
            view.FindViewById<TextView>(Resource.Id.sectionLabel).Text = label;

            if (item != null)
            {
                view.FindViewById<TextView>(Resource.Id.gearItemName).Text = item.name;

                if (item.armor != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemArmor).Text = item.armor.min.ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout).Visibility = ViewStates.Gone;

                if (item.dps != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemDps).Text = Math.Round(item.dps.min, 1).ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout).Visibility = ViewStates.Gone;

                if (item.minDamage != null && item.maxDamage != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemDamageMin).Text = item.minDamage.min.ToString();
                    view.FindViewById<TextView>(Resource.Id.gearItemDamageMax).Text = item.maxDamage.min.ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout).Visibility = ViewStates.Gone;

                if (item.attacksPerSecond != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemAttacksPerSecond).Text = Math.Round(item.attacksPerSecond.min, 2).ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout).Visibility = ViewStates.Gone;

                if (item.attributes != null)
                {
                    String description = String.Empty;
                    foreach (String s in item.attributes)
                        description += (description != String.Empty ? System.Environment.NewLine : String.Empty) + s;
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription).Text = description;
                }
                else
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription).Visibility = ViewStates.Gone;

                if (item.gems != null)
                {
                    String socketsText = String.Empty;
                    foreach (SocketedGem gem in item.gems)
                    {
                        foreach (String s in gem.attributes)
                            socketsText += (socketsText != String.Empty ? System.Environment.NewLine : String.Empty) + Application.Context.Resources.GetString(Resource.String.Socket) + " " + s;
                    }
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription).Text = socketsText;
                }
                else
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription).Visibility = ViewStates.Gone;

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

                if (icon != null)
                {
                    Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(icon.bytes, 0, (int)icon.bytes.Length);
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem).SetImageBitmap(bitmap);
                }
            }
        }
    }
}
