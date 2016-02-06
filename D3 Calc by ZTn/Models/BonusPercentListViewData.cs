namespace ZTn.Pcl.D3Calculator.Models
{
    internal class BonusPercentListViewData : ValueListViewData
    {
        public BonusPercentListViewData(string label, double value, int precision = 0)
            : base(label, 100 * value, precision)
        {
            Value = $"+ {Value} %";
        }
    }
}
