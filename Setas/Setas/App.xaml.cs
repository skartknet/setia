using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Setas.Models.Mapping;
using Setas.Services;
using Setas.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Setas
{

    public partial class App : Application
    {
        public static ImageSource SourceImage { get; set; }

        public IInternalDataService InternalDataService { get; private set; }
        public ISyncingDataService SyncingService { get; private set; }

        public static Uri ExternalService = new Uri("http://setia-dev.azurewebsites.net");
        //public static Uri ExternalService = new Uri("http://172.17.198.145:5000");

        //sync every week.
        public static TimeSpan SyncPeriod = new TimeSpan(0, 0, 5);

        public static string ApiBase = "/umbraco/api/";

        //below this limit the identification result will be displayed as 'unknown'
        public static float ProbabilityThresholdFirstResult = 60f;
        public static float ProbabilityThresholdSecondaryResults = 30f;



        //Custom Vision
        public static Guid CustomVisionProjectId = new Guid("aaedaff8-49db-48e9-aa8d-6cf0262a29d1");
        public static string CustomVisionPredictionKey = "46809c785461496aa166cf82c4bdf6b7";
        public static string PredictionEndpoint = "https://southcentralus.api.cognitive.microsoft.com";

        public static string AdUnitId
        {
            get
            {
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                    return "ca-app-pub-2003726790886919/4839520685";
                else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                    return "ca-app-pub-2003726790886919/8080051252";
                else return null;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new LoadingPage();
        }


        protected override void OnStart()
        {
            AppCenter.Start("android=86311dca-ab38-41be-bf0d-77b43d392cd4;",
                   typeof(Analytics), typeof(Crashes));

            DependencyContainer.Register();
            MappingConfiguration.Init();

            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                SyncingService = scope.Resolve<ISyncingDataService>();
                InternalDataService = scope.Resolve<IInternalDataService>();
            }

            Task.Run(async () =>
            {
                await InternalDataService.Initialise();
                await SyncingService.SyncDataAsync();
                MainPage = new MainPage();
            });


        }


        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

            if (InternalDataService.DatabaseInitialized)
            {
                Task.Run(async () =>
                {
                    await SyncingService.SyncDataAsync();
                });
            }

        }

    }



}
