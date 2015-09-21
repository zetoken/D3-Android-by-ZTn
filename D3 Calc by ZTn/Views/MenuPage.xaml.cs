using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ZTn.Pcl.D3Calculator.Views
{
    public partial class MenuPage : ContentPage
    {
        public static ObservableCollection<string> Items { get; set; }

        public MenuPage()
        {
            Items = new ObservableCollection<string>(new[] { "Menu Item 1", "Menu Item 2" });

            InitializeComponent();
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var menuItem = (string)e.SelectedItem;
            DisplayAlert("Item Selected", menuItem, "Ok");

            var listView = (ListView)sender;
            listView.SelectedItem = null;
        }
    }
}
