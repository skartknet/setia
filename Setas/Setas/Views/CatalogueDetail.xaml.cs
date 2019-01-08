using Setas.Common.Enums;
using Setas.Common.Models;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueDetail : ContentPage
    {

        public ObservableCollection<MushroomDisplayModel> Mushrooms = new ObservableCollection<MushroomDisplayModel>();

        public IInternalDataService _dataService { get; }

        //select edibles
        private Edible[] _edibles;

        private int _page = 0;


        public CatalogueDetail(IInternalDataService dataService, Edible[] edibles)
        {
            _dataService = dataService;
            _edibles = edibles;

            Task.Run(async () =>
            {
                await GetItemsAsync();
            }).Wait();

            InitializeComponent();

            DetailsList.ItemsSource = Mushrooms;

            AdView.AdUnitId = App.AdUnitId;

        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel((MushroomDisplayModel)e.Item);
            await Navigation.PushAsync(new MushroomDetail(vm));
        }

        async private void DetailsList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var item = e.Item as MushroomDisplayModel;
            var lastItem = Mushrooms.LastOrDefault();
            if (lastItem != null && lastItem.Id == item.Id)
            {
                await GetItemsAsync();
            }

        }

        async private Task GetItemsAsync()
        {
            var data = await _dataService.GetMushroomsAsync(new SearchOptions
            {
                Edibles = _edibles,
                Page = _page + 1
            });


            foreach (var item in data)
            {
                Mushrooms.Add(new MushroomDisplayModel(item));
            }

            _page++;
        }
    }
}