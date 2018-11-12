using Setas.Models;
using Setas.ViewModels;

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
    }
}