using System;
using ZTn.BNet.D3.Items;

namespace ZTnDroid.D3Calculator.Adapters
{
    public class AttributeMitigationListItem : AttributeListItem
    {
        public AttributeMitigationListItem(int resource, ItemValueRange resistance, double mitigation)
            : base(resource, String.Format("{0:N0}   ( {1:N2} % )", resistance.Min, mitigation * 100))
        {
        }
    }
}