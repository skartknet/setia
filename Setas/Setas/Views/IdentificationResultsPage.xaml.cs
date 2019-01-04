using Acr.UserDialogs;
using Setas.Models;
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


            vm.Navigation = this.Navigation;
            BindingContext = vm;

            UserDialogs.Instance.AlertAsync(new AlertConfig
            {
                Title = "Atención",
                Message = "Utiliza esta app solo como guia de campo. NUNCA comas nada que no conozcas. Siempre confirma los resultados con un experto.",
                OkText = "Entendido"              
            });

        }

        //TODO: move this to VM using behaviours. https://anthonysimmon.com/eventtocommand-in-xamarin-forms-apps/
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var model = new MushroomDetailViewModel(((Prediction)e.Item).Mushroom);

            Navigation.PushAsync(new MushroomDetail(model));
        }
    }
}