using Xamarin.Forms;

namespace ZTn.Pcl.D3Calculator.Views
{
    class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            Title = "MasterDetail Title";

            Master = new MenuPage();
            Detail = new NavigationPage(new BnetAccountsPage());
        }
    }
}
