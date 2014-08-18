using System;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class AttributeElementaryDamageListItem : AttributeListItem
    {
        public AttributeElementaryDamageListItem(int resource, ItemValueRange increase, double dps)
            : base(resource, String.Format("{0:0.00} %   ( {1:N2} )", 100 * increase.Min, dps * (1 + increase.Min)))
        {
        }
    }
}