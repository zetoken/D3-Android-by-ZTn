namespace ZTn.Pcl.D3Calculator.Models
{
    internal class TextData : IControlData
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public TextData()
        {
        }

        public TextData(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}
