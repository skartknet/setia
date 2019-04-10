using System;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();



            //var copyright = new Label
            //{
            //    Text = $"Copyright {DateTime.Today.Year} - v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}",
            //    VerticalOptions = LayoutOptions.Center,
            //    HorizontalOptions = LayoutOptions.Center
            //};
            
        }
    }
}