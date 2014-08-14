using Android.App;
using Android.Views;
using Android.Widget;
using System;
using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;
using ZTnDroid.D3Calculator.Adapters.Delegated;
using ZTnDroid.D3Calculator.Helpers;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class AttributeListItem : IListItem
    {
        public String Name;
        public String Value;

        #region >> Constructors

        protected AttributeListItem()
        {
        }

        public AttributeListItem(int id, String value) :
            this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, String value)
        {
            Name = name;
            Value = value;
        }

        public AttributeListItem(int id, HeroClass value) :
            this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, HeroClass value)
        {
            Name = name;
            Value = value.Translate();
        }

        public AttributeListItem(int id, DateTime value) :
            this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, DateTime value)
        {
            Name = name;
            Value = value.ToString();
        }

        public AttributeListItem(int id, ItemValueRange value)
            : this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, ItemValueRange value)
        {
            Name = name;
            Value = value.Min.ToString("N2");
        }

        public AttributeListItem(int id, long value)
            : this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, long value)
        {
            Name = name;
            Value = value.ToString("N0");
        }

        public AttributeListItem(int id, double value)
            : this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, double value)
        {
            Name = name;
            Value = String.Format("{0:N2}", value);
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int GetLayoutResource()
        {
            return Resource.Layout.AttributeListItem;
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
            view.FindViewById<TextView>(Resource.Id.attributeName).Text = Name;
            view.FindViewById<TextView>(Resource.Id.attributeValue).Text = Value;
        }

        #endregion
    }
}