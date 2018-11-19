﻿
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

            var identifierNavigation = new NavigationPage(new IdentificationPage());
            identifierNavigation.Title = "Identificación";
            identifierNavigation.Icon = "eyeIcon.png";

            var catalogueNavigation = new NavigationPage(new CatalogueMaster());
            catalogueNavigation.Title = "Catálogo";
            catalogueNavigation.Icon = "listIcon.png";

            var dictionaryPage = new Dictionary();
            dictionaryPage.Title = "Diccionario";
            dictionaryPage.Icon = "dictionaryIcon.png";

            Children.Add(identifierNavigation);
            Children.Add(catalogueNavigation);
            Children.Add(dictionaryPage);

        }
    }
}