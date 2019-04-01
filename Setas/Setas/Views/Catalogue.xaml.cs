using Autofac;
using Setas.Common.Enums;
using Setas.Enums;
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
            MasterPage.MenuListView.ItemSelected += ListView_ItemSelected;

            InitDefaultListingValues();

        }

        private void InitDefaultListingValues()
        {
            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                var filter = GetEdiblesClasses(EdibleTopClass.Safe);

                var listing = new CatalogueItemListing(scope.Resolve<IInternalDataService>(), filter, "Todas");

                Detail = new NavigationPage(listing);
            }
        }

        protected override void OnAppearing()
        {
            if (MasterPage.MenuListView.SelectedItem == null)
            {
                InitDefaultListingValues();
                IsPresented = false;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                if (e.SelectedItem != null)
                {
                    var menuItem = (CatalogueMenuItem)e.SelectedItem;

                    var edibles = GetEdiblesClasses(menuItem.Value);
                    var listing = new CatalogueItemListing(scope.Resolve<IInternalDataService>(), edibles, menuItem.Title);

                    Detail = new NavigationPage(listing);
                    IsPresented = false;

                }
            }

        }

        //Gets all the classes associated to an edible top classification. We don't give the change to select by any possible value.
        private Edible[] GetEdiblesClasses(EdibleTopClass? topClass)
        {
            if (topClass.HasValue)
            {
                switch (topClass)
                {
                    case EdibleTopClass.Safe:
                        return new Edible[]
                        {
                            Common.Enums.Edible.BuenComestible,
                            Common.Enums.Edible.ComestibleBajaCalidad,
                            Common.Enums.Edible.ComestibleCalidadMedia
                        };
                    case EdibleTopClass.NoInterest:
                        return new Edible[]
                        {
                            Common.Enums.Edible.SinInteres
                        };
                    case EdibleTopClass.Warning:
                        return new Edible[]{
                            Common.Enums.Edible.PosibleToxico,
                                Common.Enums.Edible.ComestibleConPrecaucion,
                                Common.Enums.Edible.ComestiblePeroPeligrosa,
                                Common.Enums.Edible.NoComestible
                            };
                    case EdibleTopClass.Toxic:
                        return new Edible[] { Common.Enums.Edible.Toxica };
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}