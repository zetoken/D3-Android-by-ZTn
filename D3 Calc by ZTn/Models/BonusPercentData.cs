namespace ZTn.Pcl.D3Calculator.Models
{
    internal class BonusPercentData : ValueData
    {
        public BonusPercentData(string label, double value, int precision = 0)
            : base(label, 100 * value, precision)
        {
            Value = $"+ {Value} %";
        }
    }
}
