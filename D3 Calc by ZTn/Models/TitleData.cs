namespace ZTn.Pcl.D3Calculator.Models
{
    internal class TitleData : IControlData
    {
        public string Title { get; set; }

        public TitleData(string title)
        {
            Title = title.ToUpper();
        }
    }
}
