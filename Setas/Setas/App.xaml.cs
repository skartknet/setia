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


        public static IInternalDataService DataService { get; set; }


        public App()
        {
            DependencyContainer.Register(this);
        }

        private void InitApp()
        {
            InitializeComponent();
            MainPage = new MainPage
            { };
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=86311dca-ab38-41be-bf0d-77b43d392cd4;",
                   typeof(Analytics), typeof(Crashes));

            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                if (DataService == null)
                {
                    try
                    {
                        DataService = scope.Resolve<IInternalDataService>();

                        var syncingService = scope.Resolve<ISyncingDataService>();
                        syncingService.SyncData();

                        InitApp();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.Alert("Error iniciando base de datos.");
                    }
                }


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
                DataService = scope.Resolve<IInternalDataService>();

                var syncingService = scope.Resolve<ISyncingDataService>();

                Task.Run(async () => await syncingService.SyncDataAsync()).Wait();
            }
        }
    }



}
