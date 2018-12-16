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

                    var edibles = GetEdiblesClasses(((CatalogueMenuItem)e.SelectedItem).Value);

                    var catalogueDetail = new CatalogueDetail(scope.Resolve<IInternalDataService>(), edibles);

                    Detail = new NavigationPage(catalogueDetail);
                    IsPresented = false;

                    MasterPage.ListView.SelectedItem = null;
                }
            }

        }

        //Gets all the classes associated to an edible top classification
        private Edible[] GetEdiblesClasses(EdibleTopClassEnum? topClass)
        {
            if (topClass.HasValue)
            {
                switch (topClass)
                {
                    case EdibleTopClassEnum.Safe:
                        return new Edible[]
                        {
                            Common.Enums.Edible.BuenComestible,
                            Common.Enums.Edible.ComestibleBajaCalidad,
                            Common.Enums.Edible.ComestibleCalidadMedia
                        };
                    case EdibleTopClassEnum.NoInterest:
                        return new Edible[]
                        {
                            Common.Enums.Edible.SinInteres
                        };
                    case EdibleTopClassEnum.Warning:
                        return new Edible[]{
                            Common.Enums.Edible.PosibleToxico,
                                Common.Enums.Edible.ComestibleConPrecaucion,
                                Common.Enums.Edible.ComestiblePeroPeligrosa,
                                Common.Enums.Edible.NoComestible
                            };
                    case EdibleTopClassEnum.Toxic:
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