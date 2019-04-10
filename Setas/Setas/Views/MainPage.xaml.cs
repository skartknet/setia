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

                var identifier = new NavigationPage(new IdentificationPage(identificationViewModel))
                {
                    Title = "Identificación",
                    Icon = "@drawable/ic_remove_red_eye_black_24dp"
                };

                var catalogue = new Catalogue
                {
                    Title = "Catálogo",
                    Icon = "@drawable/search_24"
                };

                var history = new NavigationPage(new HistoryPage(scope.Resolve<IInternalDataService>()))
                {
                    Title = "Historia",
                    Icon = "@drawable/ic_history_black_24dp"
                };

                var info = new InfoPage
                {
                    Title = "Info",
                    Icon = "@drawable/ic_help_outline_black_24dp"
                };

                Children.Add(identifier);
                Children.Add(catalogue);
                Children.Add(history);
                Children.Add(info);
            }


        }
    }
}