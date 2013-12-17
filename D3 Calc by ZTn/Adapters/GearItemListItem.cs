using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using ZTn.BNet.D3.Calculator.Helpers;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;
using ZTnDroid.D3Calculator.Adapters.Delegated;
using ZTnDroid.D3Calculator.Helpers;
using ZTnDroid.D3Calculator.Storage;


namespace ZTnDroid.D3Calculator.Adapters
{
    public class GearItemListItem : IListItem
    {
        public String Label;
        public Item Item;
        public D3Picture Icon;

        #region >> Constructors

        public GearItemListItem(String label, Item item)
        {
            Label = label;
            Item = item;
        }

        public GearItemListItem(String label, Item item, D3Picture icon)
        {
            Label = label;
            Item = item;
            Icon = icon;
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int GetLayoutResource()
        {
            return Resource.Layout.GearItemListItem;
        }

        /// <inheritdoc/>
        public bool IsEnabled()
        {
            return false;
        }

        /// <inheritdoc/>
        public void RemoveView(View view)
        {
            // Remove previous "click" EventHandler
            var editView = view.FindViewById<ImageView>(Resource.Id.gearItemEdit);
            editView.Click -= ((JavaLangObject<GearItemListItem>)view.Tag).Value.onClickEditEventHandler;
        }

        /// <inheritdoc/>
        public void UpdateView(View view, Boolean recycled)
        {
            // Store current object in View Tag property
            view.Tag = new JavaLangObject<GearItemListItem>(this);

            // Update shown informations
            view.FindViewById<TextView>(Resource.Id.sectionLabel).Text = Label;

            var editView = view.FindViewById<ImageView>(Resource.Id.gearItemEdit);
            editView.Click += onClickEditEventHandler;

            if (Item != null)
            {
                view.FindViewById<TextView>(Resource.Id.gearItemName).Text = Item.name;

                if (Item.armor != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemArmor).Text = Item.armor.min.ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout).Visibility = ViewStates.Gone;

                if (Item.dps != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemDps).Text = Math.Round(Item.dps.min, 1).ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout).Visibility = ViewStates.Gone;

                if (Item.minDamage != null && Item.maxDamage != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemDamageMin).Text = Item.minDamage.min.ToString();
                    view.FindViewById<TextView>(Resource.Id.gearItemDamageMax).Text = Item.maxDamage.min.ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout).Visibility = ViewStates.Gone;

                if (Item.attacksPerSecond != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout).Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemAttacksPerSecond).Text = Math.Round(Item.attacksPerSecond.min, 2).ToString();
                }
                else
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout).Visibility = ViewStates.Gone;

                if (Item.attributes != null)
                {
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription).Visibility = ViewStates.Visible;
                    var description = String.Empty;
                    foreach (var s in Item.attributes)
                        description += (description != String.Empty ? Environment.NewLine : String.Empty) + s;
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription).Text = description;
                }
                else
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription).Visibility = ViewStates.Gone;

                if (Item.gems != null)
                {
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription).Visibility = ViewStates.Visible;
                    var socketTranslation = Application.Context.Resources.GetString(Resource.String.Socket);
                    var socketsText = String.Empty;
                    foreach (var gem in Item.gems)
                    {
                        foreach (var s in gem.attributes)
                            socketsText += (socketsText != String.Empty ? Environment.NewLine : String.Empty) + socketTranslation + " " + s;
                    }
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription).Text = socketsText;
                }
                else
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription).Visibility = ViewStates.Gone;

                switch (Item.displayColor)
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
                    case "white":
                        view.FindViewById<TextView>(Resource.Id.gearItemName).SetTextColor(view.Resources.GetColor(Resource.Color.whiteItem));
                        break;
                }

                if (Icon != null)
                {
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem).Visibility = ViewStates.Visible;
                    var bitmap = BitmapFactory.DecodeByteArray(Icon.bytes, 0, Icon.bytes.Length);
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem).SetImageBitmap(bitmap);
                }
                else
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem).Visibility = ViewStates.Invisible;
            }
        }

        #endregion

        private void onClickEditEventHandler(Object sender, EventArgs e)
        {
            var view = (ImageView)sender;

            D3Context.Instance.EditingItem = (Item.attributesRaw == null ? Item : Item.simplify());

            var editorIntent = new Intent(view.Context, typeof(GearItemEditorActivity));

            ((Activity)view.Context).StartActivityForResult(editorIntent, GearItemEditorActivity.ItemEdit);
        }
    }
}
