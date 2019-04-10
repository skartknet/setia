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


            var contentLabel = new Label
            {
                FormattedText = new FormattedString(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 16
            };


            var thanksTitle = new Span
            {
                Text = "Agradecimientos:",
                FontAttributes = FontAttributes.Bold
            };

            var thanks = new Span
            {
                Text = "El contenido de esta aplicación ha sido proporcionado amablemente por la Asociación Cultural 'Baxauri'"
            };


            var newLine = new Span
            {
                Text = Environment.NewLine
            };

            var formattedString = new FormattedString();
            formattedString.Spans.Add(thanks);

            contentLabel.FormattedText.Spans.Add(thanksTitle);
            contentLabel.FormattedText.Spans.Add(thanks);
            contentLabel.FormattedText.Spans.Add(newLine);


            var logo = new Image()
            {
                Source = "@drawable/setia_logo",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var copyright = new Label
            {
                Text = $"Copyright {DateTime.Today.Year} - v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            ContentLayout.Children.Add(logo);
            ContentLayout.Children.Add(copyright);
            ContentLayout.Children.Add(contentLabel);
        }
    }
}