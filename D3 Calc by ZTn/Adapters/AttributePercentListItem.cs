using Android.App;
using System;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class AttributePercentListItem : AttributeListItem
    {
        #region >> Constructors

        public AttributePercentListItem(int id, long value) :
            this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributePercentListItem(String name, long value)
            : base(name, String.Format("{0} %", value))
        {
        }

        public AttributePercentListItem(int id, ItemValueRange value) :
            this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributePercentListItem(String name, ItemValueRange value) :
            base(name, String.Format("{0:0.00} %", 100 * value.min))
        {
        }

        public AttributePercentListItem(int id, double value) :
            this(Application.Context.Resources.GetString(id), value)
        {
        }

        public AttributePercentListItem(String name, double value)
            : base(name, String.Format("{0:0.00} %", 100 * value))
        {
        }

        #endregion
    }
}