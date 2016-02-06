using System;

namespace ZTn.Pcl.D3Calculator.Models
{
    internal class ValueListViewData : TextListViewData
    {
        public ValueListViewData(string label, double value, int precision = 0)
            : base(label, String.Format($"{{0:N{precision}}}", value))
        {
        }
    }
}
