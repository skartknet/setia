
using Autofac;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Setas.Services;
using Setas.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom)
                         .SetBarItemColor(Color.FromRgb(173, 112, 69))
                         .SetBarSelectedItemColor(Color.Red);

            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {                
                var predictionClient = scope.Resolve<ICustomVisionPredictionClient>();
                predictionClient.ApiKey = App.CustomVisionPredictionKey;
                predictionClient.Endpoint = App.PredictionEndpoint;

                var identificationViewModel = new IdentificationViewModel(scope.Resolve<IInternalDataService>(), predictionClient);

                var identifierNavigation = new NavigationPage(new IdentificationPage(identificationViewModel))
                {
                    Title = "Identificación",
                    Icon = "eyeIcon.png"
                };

                var catalogueNavigation = new Catalogue();
                catalogueNavigation.Title = "Catálogo";
                catalogueNavigation.Icon = "listIcon.png";

                var history = new NavigationPage(new HistoryPage(scope.Resolve<IInternalDataService>()));
                history.Title = "Historia";
                history.Icon = "eyeIcon.png";

                Children.Add(identifierNavigation);
                Children.Add(catalogueNavigation);
                Children.Add(history);
            }


        }
    }
}