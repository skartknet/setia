using Acr.UserDialogs;
using Setas.ViewModels;
using Xamarin.Forms;

namespace Setas
{
    public partial class IdentificationPage : ContentPage
    {

        private IdentificationViewModel _vm { get; }

        public IdentificationPage(IdentificationViewModel vm)
        {
            _vm = vm;
            vm.Navigation = this.Navigation;

            InitializeComponent();

            BindingContext = vm;            

        }


        protected override void OnAppearing()
        {
            UserDialogs.Instance.HideLoading();
        }

    }
}