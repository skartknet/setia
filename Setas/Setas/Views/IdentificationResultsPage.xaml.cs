using Setas.ViewModels;
using Setas.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IdentificationResultsPage : ContentPage
    {

        ResultsViewModel vm;

        internal IdentificationResultsPage()
        {            

            InitializeComponent();

        }


        internal IdentificationResultsPage(ResultsViewModel _vm)
            : this()
        {
            vm = _vm;
            BindingContext = vm;

        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var model = new MushroomDetailViewModel()
            {
                Name = "Lentinus edodes",
                PopularNames = "Shiitake"
            };

            Navigation.PushAsync(new MushroomDetail(model));
        }
    }
}