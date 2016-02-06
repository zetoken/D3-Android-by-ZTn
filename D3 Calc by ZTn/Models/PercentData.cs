namespace ZTn.Pcl.D3Calculator.Models
{
    internal class PercentData : ValueData
    {
        public PercentData(string label, double value, int precision = 0)
            : base(label, 100 * value, precision)
        {
            Value = $"{Value} %";
        }
    }
}