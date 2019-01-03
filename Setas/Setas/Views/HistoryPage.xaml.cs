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
		public HistoryPage (IInternalDataService dataService)
		{
            var history = Task.Run(async () => await dataService.GetHistoryAsync()).Result;
			InitializeComponent ();


            HistoryList.ItemsSource = history;

        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel(new MushroomDisplayModel((MushroomData)e.Item));
            await Navigation.PushAsync(new MushroomDetail(vm));
        }
    }
}