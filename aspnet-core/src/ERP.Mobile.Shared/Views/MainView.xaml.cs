using Xamarin.Forms;

namespace ERP.Views
{
    public partial class MainView : MasterDetailPage, IXamarinView
    {
        public MainView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
