using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTn.BNet.D3.Medias;
using ZTnDroid.D3Calculator.Adapters.Delegated;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class PowerListItem : IListItem
    {
        public LegendaryPower Power;
        public D3Picture Icon;

        #region >> Constructors

        public PowerListItem(LegendaryPower power)
        {
            Power = power;
        }

        public PowerListItem(LegendaryPower power, D3Picture icon)
        {
            Power = power;
            Icon = icon;
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int GetLayoutResource()
        {
            return Resource.Layout.PowerListItem;
        }

        /// <inheritdoc/>
        public bool IsEnabled()
        {
            return false;
        }

        /// <inheritdoc/>
        public void RemoveView(View view)
        {
        }

        /// <inheritdoc/>
        public void UpdateView(View view, Boolean recycled)
        {
            view.FindViewById<TextView>(Resource.Id.powerName).Text = Power.Name;
            view.FindViewById<TextView>(Resource.Id.powerDescription).Text = "";
            if (Icon != null)
            {
                var bitmap = BitmapFactory.DecodeByteArray(Icon.Bytes, 0, Icon.Bytes.Length);
                view.FindViewById<ImageView>(Resource.Id.imagePower).SetImageBitmap(bitmap);
            }
        }

        #endregion
    }
}
