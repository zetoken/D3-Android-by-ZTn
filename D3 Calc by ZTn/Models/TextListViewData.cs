namespace ZTn.Pcl.D3Calculator.Models
{
    internal class TextListViewData : IListViewRowData
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public TextListViewData()
        {
        }

        public TextListViewData(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}
