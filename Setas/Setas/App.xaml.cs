using Setas.Enums;
using Setas.Services;
using Setas.Views;
using System;
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

            if (!Properties.ContainsKey("dataType") || !Enum.TryParse(this.Properties["dataType"].ToString(), out dataServiceType))
            {
                Properties["dataType"] = SettingsDataType.External;
                dataServiceType = SettingsDataType.External;
            }

            DependencyContainer.Register(this);

            InitializeComponent();
            MainPage = new MainPage
            {

            };


        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }



}
