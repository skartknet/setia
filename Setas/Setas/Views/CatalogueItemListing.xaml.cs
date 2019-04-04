using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Setas.Common.Enums;
using Setas.Common.Models;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueItemListing : ContentPage
    {

        public ObservableCollection<MushroomDisplayModel> Mushrooms = new ObservableCollection<MushroomDisplayModel>();

        public IInternalDataService _dataService { get; }

        //selected edibles
        private Edible[] filter;

        private int _page = 0;

        public CatalogueItemListing(IInternalDataService dataService)
        {

            _dataService = dataService;

            InitializeComponent();

            BindingContext = this;
            AdView.AdUnitId = App.AdUnitId;

        }

        public async Task FilterList(Edible[] ediblesFilter, string pageTitle)
        {

            _page = 0;
            this.filter = ediblesFilter;
            Title = pageTitle;

            Mushrooms.Clear();

            await GetItemsAsync();

        }



        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel((MushroomDisplayModel)e.Item);
            await Navigation.PushAsync(new MushroomDetailPage(vm));
        }

        async void DetailsList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var item = e.Item as MushroomDisplayModel;
            var lastItem = Mushrooms.LastOrDefault();
            if (lastItem != null && lastItem.Id == item.Id)
            {
                await GetItemsAsync();
            }
        }

        private async Task GetItemsAsync()
        {
            var data = await _dataService.GetMushroomsAsync(new SearchOptions
            {
                Edibles = filter,
                Page = _page + 1
            });


            foreach (var item in data)
            {
                Mushrooms.Add(new MushroomDisplayModel(item));
            }

            DetailsList.ItemsSource = Mushrooms;

            _page++;
        }
    }
}