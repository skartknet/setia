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
            if (DesignMode.IsDesignModeEnabled)
            {
                var _vm = new ResultsViewModel
                {
                    FirstResult = new Prediction
                    {
                        TagName = "Seta 1",
                        Probability = 55.5f
                    },
                    SecondaryResults = new Prediction[]
                    {
                        new Prediction
                        {
                            TagName ="Seta 2",
                            Probability = 55.5f
                        },
                        new Prediction
                        {
                            TagName ="Seta 3",
                            Probability = 38f
                        },
                        new Prediction
                        {
                            TagName ="Seta 4",
                            Probability = 10f
                        }
                    }
                };
            }

            InitializeComponent();
            BindingContext = vm;

        }


        internal IdentificationResultsPage(ResultsViewModel _vm)
            : this()
        {
            vm = _vm;
        }
    }
}