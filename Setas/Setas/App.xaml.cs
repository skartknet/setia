using Acr.UserDialogs;
using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
        public static Uri ExternalService = new Uri("http://setia-dev.azurewebsites.net");
        public static string ApiBase = "/umbraco/api/";

        public static IInternalDataService DataService { get; set; }


        public App()
        {
            DependencyContainer.Register(this);
        }


        protected override void OnStart()
        {

            AppCenter.Start("android=86311dca-ab38-41be-bf0d-77b43d392cd4;",
                   typeof(Analytics), typeof(Crashes));

            InitDatabase();
        }


        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {            
        }

        private void InitDatabase()
        {
            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                if (DataService == null)
                {
                    //UserDialogs.Instance.Loading("Iniciando base de datos...", null, null, true, MaskType.Clear));
                 
                    Task.Run(async () =>
                    {
                        try
                        {
                            DataService = scope.Resolve<IInternalDataService>();

                            var syncingService = scope.Resolve<ISyncingDataService>();

                            await syncingService.SyncDataAsync();
                            InitApp();

                        }
                        catch (Exception ex)
                        {
                            await UserDialogs.Instance.AlertAsync("Error iniciando base de datos.");
                        }
                    }).Wait();


                }
            }
        }

        


        private void InitApp()
        {
            UserDialogs.Instance.HideLoading();

            InitializeComponent();
            MainPage = new MainPage();
        }
    }



}
