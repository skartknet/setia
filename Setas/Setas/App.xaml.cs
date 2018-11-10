using Setas.Models;
using Setas.Views;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Setas
{

    public partial class App : Application
    {
        public static Prediction FirstPrediction { get; set; }
        public static IEnumerable<Prediction> SecondaryPredictions { get; set; }
        public static ImageSource SourceImage { get; set; }

        public App()
        {
            if (DesignMode.IsDesignModeEnabled)
            {

                FirstPrediction = new Prediction
                {
                    TagName = "Predi1",
                    Probability = 55.5f
                };
                SecondaryPredictions = new Prediction[]
                {
                    new Prediction
                    {
                        TagName ="Predi1",
                        Probability = 55.5f
                    },
                    new Prediction
                    {
                        TagName ="Predi1",
                        Probability = 38f
                    },
                    new Prediction
                    {
                        TagName ="Predi1",
                        Probability = 10f
                    }
                };
            }

            InitializeComponent();
            MainPage = new SetasNavigationPage(new IdentificationPage());


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
