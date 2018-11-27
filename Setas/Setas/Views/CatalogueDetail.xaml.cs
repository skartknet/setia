using Setas.Common.Enums;
using Setas.Common.Models;
using Setas.Services;
using Setas.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueDetail : ContentPage
    {        

        public ObservableCollection<Mushroom> Mushrooms;

        public IInternalDataService _dataService { get; }



        public CatalogueDetail(IInternalDataService dataService, Edible? edible)
        {
            _dataService = dataService;

            Task.Run(async () =>
            {
                Mushrooms = new ObservableCollection<Mushroom>(await _dataService.GetMushroomsAsync(new SearchOptions
                {
                    Edible = edible
                }));
                
            }).Wait();

            InitializeComponent();

            DetailsList.ItemsSource = Mushrooms;

        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel((Mushroom)e.Item);
            await Navigation.PushAsync(new MushroomDetail(vm));
        }
    }
}