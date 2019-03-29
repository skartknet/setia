using Setas.Common.Models;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : ContentPage
	{
        private IInternalDataService _dataService { get; }

		public HistoryPage (IInternalDataService dataService)
		{
            _dataService = dataService;
			InitializeComponent ();

            AdView.AdUnitId = App.AdUnitId;
        }


        protected override void OnAppearing()
        {
            var history = Task.Run(async () => await _dataService.GetHistoryAsync()).Result;
            HistoryList.ItemsSource =  history.Select(h=>new HistoryItemDisplayModel() {
                TakenOn= h.TakenOn.ToShortDateString(),
                MushroomId = h.MushroomId,
                Mushroom = new MushroomDisplayModel(h.Mushroom)
            });
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel(new MushroomDisplayModel(((HistoryItemDisplayModel)e.Item).Mushroom));
            await Navigation.PushAsync(new MushroomDetailPage(vm));
        }
    }
}