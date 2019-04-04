using Setas.Services;
using Setas.ViewModels;
using Setas.ViewModels.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueMenu : ContentPage
    {
        public ListView MenuListView;
        public CatalogueMenu()
        {
            InitializeComponent();

            BindingContext = new CatalogueMasterViewModel();

            MenuListView = MenuItemsListView;

        }

       
    }
}