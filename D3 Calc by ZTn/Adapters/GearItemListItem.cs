using System;
using System.Globalization;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
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
            view.FindViewById<TextView>(Resource.Id.sectionLabel)
                .Text = Label;

            var editView = view.FindViewById<ImageView>(Resource.Id.gearItemEdit);
            editView.Click += onClickEditEventHandler;

            if (Item != null)
            {
                view.FindViewById<TextView>(Resource.Id.gearItemName)
                    .Text = Item.Name;

                if (Item.Armor != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout)
                        .Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemArmor)
                        .Text = Item.Armor.Min.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout)
                        .Visibility = ViewStates.Gone;
                }

                if (Item.Dps != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout)
                        .Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemDps)
                        .Text = Math.Round(Item.Dps.Min, 1)
                            .ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout)
                        .Visibility = ViewStates.Gone;
                }

                if (Item.MinDamage != null && Item.MaxDamage != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout)
                        .Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemDamageMin)
                        .Text = Item.MinDamage.Min.ToString(CultureInfo.CurrentCulture);
                    view.FindViewById<TextView>(Resource.Id.gearItemDamageMax)
                        .Text = Item.MaxDamage.Min.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout)
                        .Visibility = ViewStates.Gone;
                }

                if (Item.AttacksPerSecond != null)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout)
                        .Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearItemAttacksPerSecond)
                        .Text = Math.Round(Item.AttacksPerSecond.Min, 2)
                            .ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout)
                        .Visibility = ViewStates.Gone;
                }

                if (Item.Attributes != null)
                {
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription)
                        .Visibility = ViewStates.Visible;
                    var description = String.Empty;
                    if (Item.Attributes.Primary != null)
                    {
                        description = Item.Attributes.Primary.Aggregate(description,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + s.Text
                            );
                    }
                    if (Item.Attributes.Secondary != null)
                    {
                        description = Item.Attributes.Secondary.Aggregate(description,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + s.Text
                            );
                    }
                    if (Item.Attributes.Passive != null)
                    {
                        description = Item.Attributes.Passive.Aggregate(description,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + s.Text
                            );
                    }
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription)
                        .Text = description;
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.gearItemDescription)
                        .Visibility = ViewStates.Gone;
                }

                if (Item.Gems != null)
                {
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription)
                        .Visibility = ViewStates.Visible;
                    var socketTranslation = Application.Context.Resources.GetString(Resource.String.Socket);
                    var socketsText = String.Empty;
                    foreach (var gem in Item.Gems)
                    {
                        if (Item.Attributes.Primary != null)
                        {
                            socketsText = gem.Attributes.Primary.Aggregate(socketsText,
                                (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + socketTranslation + " " + s.Text
                                );
                        }
                        if (Item.Attributes.Secondary != null)
                        {
                            socketsText = gem.Attributes.Secondary.Aggregate(socketsText,
                                (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + socketTranslation + " " + s.Text
                                );
                        }
                        if (Item.Attributes.Passive != null)
                        {
                            socketsText = gem.Attributes.Passive.Aggregate(socketsText,
                                (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + socketTranslation + " " + s.Text
                                );
                        }
                    }
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription)
                        .Text = socketsText;
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.gearSocketsDescription)
                        .Visibility = ViewStates.Gone;
                }

                switch (Item.DisplayColor)
                {
                    case "orange":
                        view.FindViewById<TextView>(Resource.Id.gearItemName)
                            .SetTextColor(view.Resources.GetColor(Resource.Color.orangeItem));
                        break;
                    case "yellow":
                        view.FindViewById<TextView>(Resource.Id.gearItemName)
                            .SetTextColor(view.Resources.GetColor(Resource.Color.yellowItem));
                        break;
                    case "green":
                        view.FindViewById<TextView>(Resource.Id.gearItemName)
                            .SetTextColor(view.Resources.GetColor(Resource.Color.greenItem));
                        break;
                    case "blue":
                        view.FindViewById<TextView>(Resource.Id.gearItemName)
                            .SetTextColor(view.Resources.GetColor(Resource.Color.blueItem));
                        break;
                    case "white":
                        view.FindViewById<TextView>(Resource.Id.gearItemName)
                            .SetTextColor(view.Resources.GetColor(Resource.Color.whiteItem));
                        break;
                }

                if (Icon != null)
                {
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem)
                        .Visibility = ViewStates.Visible;
                    var bitmap = BitmapFactory.DecodeByteArray(Icon.Bytes, 0, Icon.Bytes.Length);
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem)
                        .SetImageBitmap(bitmap);
                }
                else
                {
                    view.FindViewById<ImageView>(Resource.Id.imageGearItem)
                        .Visibility = ViewStates.Invisible;
                }
            }
        }

        #endregion

        private void onClickEditEventHandler(Object sender, EventArgs e)
        {
            var view = (ImageView)sender;

            D3Context.Instance.EditingItem = (Item.AttributesRaw == null ? Item : Item.Simplify());

            var editorIntent = new Intent(view.Context, typeof(GearItemEditorActivity));

            ((Activity)view.Context).StartActivityForResult(editorIntent, GearItemEditorActivity.ItemEdit);
        }
    }
}