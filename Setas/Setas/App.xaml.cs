using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Setas.Enums;
using Setas.Services;
using Setas.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Setas
{

    public partial class App : Application
    {
        public static ImageSource SourceImage { get; set; }


        public readonly SettingsDataType dataServiceType;


        public App()
        {


            DependencyContainer.Register(this);

            InitializeComponent();
            MainPage = new MainPage
            {

            };


        }

        protected override void OnStart()
        {
            AppCenter.Start("android=86311dca-ab38-41be-bf0d-77b43d392cd4;",
                   typeof(Analytics), typeof(Crashes));

            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                var syncingService = scope.Resolve<ISyncingDataService>();
                Task.Run(async () => await syncingService.SyncDataAsync());
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                var syncingService = scope.Resolve<ISyncingDataService>();

                Task.Run(async () => await syncingService.SyncDataAsync());
            }
        }
    }



}
