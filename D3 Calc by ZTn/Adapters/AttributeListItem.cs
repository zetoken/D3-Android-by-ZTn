using System;

using Android.Views;
using Android.Widget;

using ZTn.BNet.D3.Heroes;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class AttributeListItem : IListItem
    {
        public String name;
        public String value;

        #region >> Constructors

        protected AttributeListItem()
        {
        }

        public AttributeListItem(int id, String value) :
            this(D3Calc.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        public AttributeListItem(int id, HeroClass value) :
            this(D3Calc.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, HeroClass value)
        {
            this.name = name;
            switch (value)
            {
                case HeroClass.Barbarian:
                    this.value = D3Calc.Context.Resources.GetString(Resource.String.barbarian);
                    break;
                case HeroClass.DemonHunter:
                    this.value = D3Calc.Context.Resources.GetString(Resource.String.demonHunter);
                    break;
                case HeroClass.Monk:
                    this.value = D3Calc.Context.Resources.GetString(Resource.String.monk);
                    break;
                case HeroClass.WitchDoctor:
                    this.value = D3Calc.Context.Resources.GetString(Resource.String.witchDoctor);
                    break;
                case HeroClass.Wizard:
                    this.value = D3Calc.Context.Resources.GetString(Resource.String.wizard);
                    break;
                default:
                    this.value = "Unknown";
                    break;
            }
        }

        public AttributeListItem(int id, DateTime value) :
            this(D3Calc.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, DateTime value)
        {
            this.name = name;
            this.value = value.ToString();
        }

        public AttributeListItem(int id, ItemValueRange value)
            : this(D3Calc.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, ItemValueRange value)
        {
            this.name = name;
            this.value = value.min.ToString();
        }

        public AttributeListItem(int id, long value)
            : this(D3Calc.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, long value)
        {
            this.name = name;
            this.value = value.ToString();
        }

        public AttributeListItem(int id, double value)
            : this(D3Calc.Context.Resources.GetString(id), value)
        {
        }

        public AttributeListItem(String name, double value)
        {
            this.name = name;
            this.value = String.Format("{0:0.00}", value);
        }

        #endregion

        #region >> IListItem

        /// <inheritdoc/>
        public int getLayoutResource()
        {
            return Resource.Layout.AttributeListItem;
        }

        /// <inheritdoc/>
        public bool isEnabled()
        {
            return false;
        }

        /// <inheritdoc/>
        public void removeView(View view)
        {
        }

        /// <inheritdoc/>
        public void updateView(View view, Boolean recycled)
        {
            view.FindViewById<TextView>(Resource.Id.attributeName).Text = name;
            view.FindViewById<TextView>(Resource.Id.attributeValue).Text = value;
        }

        #endregion
    }
}