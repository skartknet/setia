

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

using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;
using System;
using Microsoft.AppCenter.Crashes;
using Acr.UserDialogs;

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
        private long _totalItems;

        public CatalogueItemListing(IInternalDataService dataService)
        {

            _dataService = dataService;

            InitializeComponent();

            BindingContext = this;
            AdView.AdUnitId = App.AdUnitId;



        }

        public async Task FilterList(Edible[] ediblesFilter, string pageTitle)
        {
            this.filter = ediblesFilter;
            pageTitleView.Text = pageTitle;

            ClearResults();
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
            if (lastItem != null && lastItem.Id == item.Id && Mushrooms.Count() < _totalItems)
            {
                _page++;

                await GetItemsAsync();
            }
        }

        private async Task GetItemsAsync(string query = null)
        {

            var options = new SearchOptions
            {
                Edibles = filter,
                Page = _page + 1,
                PageSize = 10,
                QueryTerm = query
            };


            var data = await GetPagedResultsAsync(options);
            _totalItems = data.TotalItems;

            foreach (var item in data.Items)
            {
                Mushrooms.Add(new MushroomDisplayModel(item));
            }

            DetailsList.ItemsSource = Mushrooms;

        }

        private async Task<PagedResult<Data.Mushroom>> GetPagedResultsAsync(SearchOptions options)
        {
            PagedResult<Data.Mushroom> pagedResult = null;
            try
            {
                var result = await _dataService.GetMushroomsAsync(options);
                var totalItems = await _dataService.GetTotalCountAsync(options);

                pagedResult = new PagedResult<Data.Mushroom>(totalItems, options.Page, options.PageSize)
                {
                    Items = result
                };

                return pagedResult;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                UserDialogs.Instance.Alert($"Error connecting to database.");
            }

            return pagedResult;
        }

        private async void SearchBar_SearchButtonPressed(object sender, System.EventArgs e)
        {

            ClearResults();
            await GetItemsAsync(((SearchBar)sender).Text);
        }

        private void ClearResults()
        {
            Mushrooms.Clear();
            _page = 0;
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = (SearchBar)sender;
            if (string.IsNullOrEmpty(search.Text))
            {
                ClearResults();
                await GetItemsAsync();
            }
        }
    }
}