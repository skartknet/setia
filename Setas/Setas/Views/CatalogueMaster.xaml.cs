using Setas.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueMaster : ContentPage
    {
        public ListView MenuListView;

        public CatalogueMaster()
        {
            InitializeComponent();

            BindingContext = new CatalogueMasterViewModel();
            MenuListView = MenuItemsListView;
        }

    }
}