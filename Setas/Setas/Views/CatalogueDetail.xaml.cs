using Autofac;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueDetail : ContentPage
    {
        public CatalogueDetail()
        {
            InitializeComponent();

            using (var scope = DependencyContainer.Container.BeginLifetimeScope())
            {
                var vm = new MushroomsListingViewModel(scope.Resolve<IDataService>());
                BindingContext = vm;
                vm.GetListingAsync();
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel((Mushroom)e.Item);
            Navigation.PushAsync(new MushroomDetail(vm));
        }
    }
}