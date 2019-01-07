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

        public string AdUnitId
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                    return "ca-app-pub-2003726790886919/4839520685";
                else if (Device.RuntimePlatform == Device.Android)
                    return "ca-app-pub-2003726790886919/3499977722";
                else return null;
            }
        }

        private IInternalDataService _dataService { get; }

		public HistoryPage (IInternalDataService dataService)
		{
			InitializeComponent ();
            _dataService = dataService;
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
            await Navigation.PushAsync(new MushroomDetail(vm));
        }
    }
}