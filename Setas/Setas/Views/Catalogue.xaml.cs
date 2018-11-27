using Autofac;
using Setas.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Catalogue : MasterDetailPage
    {
        public Catalogue()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                Detail = new NavigationPage(new CatalogueDetail(scope.Resolve<IInternalDataService>(), null));
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                if (e.SelectedItem != null)
                {
                    var catalogueDetail = new CatalogueDetail(scope.Resolve<IInternalDataService>(), ((CatalogueMenuItem)e.SelectedItem).Id);

                    Detail = new NavigationPage(catalogueDetail);
                    IsPresented = false;

                    MasterPage.ListView.SelectedItem = null;
                }
            }

        }
    }
}