using System;
using System.Diagnostics;
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
        private readonly string label;
        private readonly Item item;
        private readonly D3Picture icon;

        #region >> Constructors

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
                .Text = label;

            var editView = view.FindViewById<ImageView>(Resource.Id.gearItemEdit);
            editView.Click += onClickEditEventHandler;

            if (item == null)
            {
                editView.Visibility = ViewStates.Gone;
                return;
            }

            editView.Visibility = item.AttributesRaw != null ? ViewStates.Visible : ViewStates.Gone;

            view.FindViewById<TextView>(Resource.Id.gearItemName)
                .Text = item.Name;

            if (item.Armor != null)
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout)
                    .Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.gearItemArmor)
                    .Text = item.Armor.Min.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemArmorLayout)
                    .Visibility = ViewStates.Gone;
            }

            if (item.Dps != null)
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout)
                    .Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.gearItemDps)
                    .Text = Math.Round(item.Dps.Min, 1)
                        .ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemDpsLayout)
                    .Visibility = ViewStates.Gone;
            }

            if (item.MinDamage != null && item.MaxDamage != null)
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout)
                    .Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.gearItemDamageMin)
                    .Text = item.MinDamage.Min.ToString(CultureInfo.CurrentCulture);
                view.FindViewById<TextView>(Resource.Id.gearItemDamageMax)
                    .Text = item.MaxDamage.Min.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemDamageLayout)
                    .Visibility = ViewStates.Gone;
            }

            if (item.AttacksPerSecond != null)
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout)
                    .Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.gearItemAttacksPerSecond)
                    .Text = Math.Round(item.AttacksPerSecond.Min, 2)
                        .ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearItemAttacksPerSecondLayout)
                    .Visibility = ViewStates.Gone;
            }

            if (item.Attributes != null)
            {
                UpdateViewWithAttributes(view);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.gearPrimaryTitle)
                    .Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.gearPrimaryDescription)
                    .Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.gearSecondaryTitle)
                    .Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.gearSecondaryDescription)
                    .Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.gearPassiveDescription)
                    .Visibility = ViewStates.Gone;
            }

            if (item.Gems != null && item.Gems.Any())
            {
                UpdateViewWithGems(view);
            }
            else
            {
                view.FindViewById<LinearLayout>(Resource.Id.gearJewelLayout)
                    .Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.gearJewelDescription)
                    .Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.gearSocketsDescription)
                    .Visibility = ViewStates.Gone;
            }

            view.FindViewById<TextView>(Resource.Id.gearItemName)
                        .SetTextColor(GetItemColor());

            if (icon != null)
            {
                UpdateViewWithIcon(view);
            }
            else
            {
                view.FindViewById<ImageView>(Resource.Id.imageGearItem)
                    .Visibility = ViewStates.Invisible;
            }
        }

        #endregion

        private void onClickEditEventHandler(object sender, EventArgs e)
        {
            var view = (ImageView)sender;

            D3Context.Instance.EditingItem = (item.AttributesRaw == null ? item : item.Simplify());

            var editorIntent = new Intent(view.Context, typeof(GearItemEditorActivity));

            ((Activity)view.Context).StartActivityForResult(editorIntent, GearItemEditorActivity.ItemEdit);
        }

        private void UpdateViewWithAttributes(View view)
        {
            Debug.Assert(item.Attributes != null);

            var primaryTitle = view.FindViewById<TextView>(Resource.Id.gearPrimaryTitle);
            var primary = view.FindViewById<TextView>(Resource.Id.gearPrimaryDescription);

            if (item.Attributes.Primary != null && item.Attributes.Primary.Any())
            {
                primaryTitle.Visibility = ViewStates.Visible;
                primary.Visibility = ViewStates.Visible;
                primary.Text = item.Attributes.Primary.Aggregate(String.Empty,
                    (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + s.Text
                    );
            }
            else
            {
                primaryTitle.Visibility = ViewStates.Gone;
                primary.Visibility = ViewStates.Gone;
            }

            var secondaryTitle = view.FindViewById<TextView>(Resource.Id.gearSecondaryTitle);
            var secondary = view.FindViewById<TextView>(Resource.Id.gearSecondaryDescription);

            if (item.Attributes.Secondary != null && item.Attributes.Secondary.Any())
            {
                secondaryTitle.Visibility = ViewStates.Visible;
                secondary.Visibility = ViewStates.Visible;
                secondary.Text = item.Attributes.Secondary.Aggregate(String.Empty,
                    (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + s.Text
                    );
            }
            else
            {
                secondaryTitle.Visibility = ViewStates.Gone;
                secondary.Visibility = ViewStates.Gone;
            }

            var passive = view.FindViewById<TextView>(Resource.Id.gearPassiveDescription);
            if (item.Attributes.Passive != null && item.Attributes.Passive.Any())
            {
                passive.Visibility = ViewStates.Visible;
                passive.Text = item.Attributes.Passive.Aggregate(String.Empty,
                    (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + s.Text
                    );
            }
            else
            {
                passive.Visibility = ViewStates.Gone;
            }
        }

        private void UpdateViewWithGems(View view)
        {
            Debug.Assert(item.Gems != null);

            var jewelLayout = view.FindViewById<LinearLayout>(Resource.Id.gearJewelLayout);
            var jewelDescription = view.FindViewById<TextView>(Resource.Id.gearJewelDescription);

            jewelLayout.Visibility = ViewStates.Gone;
            jewelDescription.Visibility = ViewStates.Gone;

            var socketsText = String.Empty;

            foreach (var gem in item.Gems)
            {
                if (gem.IsJewel)
                {
                    var jewelText = "";
                    if (item.Attributes.Primary != null)
                    {
                        jewelText = gem.Attributes.Primary.Aggregate(jewelText,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + " " + s.Text
                            );
                    }
                    if (item.Attributes.Secondary != null)
                    {
                        jewelText = gem.Attributes.Secondary.Aggregate(jewelText,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + " " + s.Text
                            );
                    }
                    if (item.Attributes.Passive != null)
                    {
                        jewelText = gem.Attributes.Passive.Aggregate(jewelText,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + " " + s.Text
                            );
                    }

                    jewelLayout.Visibility = ViewStates.Visible;
                    view.FindViewById<TextView>(Resource.Id.gearJewelName)
                        .Text = gem.Item.Name;
                    view.FindViewById<TextView>(Resource.Id.gearJewelRank)
                        .Text = gem.JewelRank.ToString();

                    jewelDescription.Visibility = ViewStates.Visible;
                    jewelDescription.Text = jewelText;
                }
                else
                {
                    if (item.Attributes.Primary != null)
                    {
                        socketsText = gem.Attributes.Primary.Aggregate(socketsText,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + " " + s.Text
                            );
                    }
                    if (item.Attributes.Secondary != null)
                    {
                        socketsText = gem.Attributes.Secondary.Aggregate(socketsText,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + " " + s.Text
                            );
                    }
                    if (item.Attributes.Passive != null)
                    {
                        socketsText = gem.Attributes.Passive.Aggregate(socketsText,
                            (current, s) => current + (current != String.Empty ? Environment.NewLine : String.Empty) + " " + s.Text
                            );
                    }
                }
            }

            var socketDescriptionView = view.FindViewById<TextView>(Resource.Id.gearSocketsDescription);
            if (!String.IsNullOrWhiteSpace(socketsText))
            {
                socketDescriptionView.Visibility = ViewStates.Visible;
                socketDescriptionView.Text = socketsText;
            }
            else
            {
                socketDescriptionView.Visibility = ViewStates.Gone;
            }
        }

        private void UpdateViewWithIcon(View view)
        {
            Debug.Assert(icon != null);

            var imageView = view.FindViewById<ImageView>(Resource.Id.imageGearItem);

            imageView.Visibility = ViewStates.Visible;

            var bitmap = BitmapFactory.DecodeByteArray(icon.Bytes, 0, icon.Bytes.Length);
            imageView.SetImageBitmap(bitmap);

            if (item.IsAncient())
            {
                imageView.SetBackgroundResource(Resource.Drawable.ancientborder);
            }
            else
            {
                imageView.Background = null;
            }
        }

        private Color GetItemColor()
        {
            switch (item.DisplayColor)
            {
                case "orange":
                    return D3Calc.Instance.Resources.GetColor(Resource.Color.orangeItem);
                case "yellow":
                    return D3Calc.Instance.Resources.GetColor(Resource.Color.yellowItem);
                case "green":
                    return D3Calc.Instance.Resources.GetColor(Resource.Color.greenItem);
                case "blue":
                    return D3Calc.Instance.Resources.GetColor(Resource.Color.blueItem);
                case "white":
                    return D3Calc.Instance.Resources.GetColor(Resource.Color.whiteItem);
                default:
                    return D3Calc.Instance.Resources.GetColor(Resource.Color.whiteItem);
            }
        }
    }
}