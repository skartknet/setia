using Autofac;
using Setas.Common.Enums;
using Setas.Enums;
using Setas.Services;
using Setas.ViewModels.Helpers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Catalogue : MasterDetailPage
    {

        private EdibleTopClass? selectedOption = null;
        CatalogueMenu masterPage;
        NavigationPage detailsPage;
        CatalogueItemListing listPage;

        public Catalogue()
        {
            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                masterPage = new CatalogueMenu();
                masterPage.MenuListView.ItemSelected += ListView_ItemSelected;

                listPage = new CatalogueItemListing(scope.Resolve<IInternalDataService>());
                detailsPage = new NavigationPage(listPage);

                Master = masterPage;
                Detail = detailsPage;

                InitializeComponent();

            }

        }

        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                await DisplayItemsList();
            });
        }



        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem != null)
            {
                var menuItem = (CatalogueMenuItem)e.SelectedItem;
                selectedOption = menuItem.Value;

                await DisplayItemsList();
            }

        }

        private async Task DisplayItemsList()
        {

            var filter = EdibleHelpers.EdibleTopClassToEdibles(selectedOption);

            await listPage.FilterList(filter, EdibleHelpers.EdibleTopClassToReadableString(selectedOption));

            IsPresented = false;

        }

    }
}