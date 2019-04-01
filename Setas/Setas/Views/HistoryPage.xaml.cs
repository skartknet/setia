using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        private IInternalDataService _dataService { get; }

        public ObservableCollection<HistoryItemDisplayModel> Mushrooms = new ObservableCollection<HistoryItemDisplayModel>();

        public HistoryPage(IInternalDataService dataService)
        {
            _dataService = dataService;
            InitializeComponent();

            AdView.AdUnitId = App.AdUnitId;
        }


        protected override void OnAppearing()
        {
            var history = Task.Run(async () => await _dataService.GetHistoryAsync()).Result;
            var items = history.Select(h => new HistoryItemDisplayModel()
            {
                TakenOn = h.TakenOn.ToShortDateString(),
                MushroomId = h.MushroomId,
                Mushroom = new MushroomDisplayModel(h.Mushroom)
            });

            foreach (var item in items)
            {
                Mushrooms.Add(item);
            }
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel(new MushroomDisplayModel(((HistoryItemDisplayModel)e.Item).Mushroom));
            await Navigation.PushAsync(new MushroomDetailPage(vm));
        }
    }
}