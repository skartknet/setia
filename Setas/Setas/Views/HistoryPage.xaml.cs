using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
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
            Task.Run(async () => await _dataService.GetHistoryAsync()).ContinueWith((t) =>
            {
                using (UserDialogs.Instance.Loading("Cargando..."))
                {
                    var items = t.Result.Select(h => new HistoryItemDisplayModel()
                    {
                        TakenOn = h.TakenOn.ToShortDateString(),
                        MushroomId = h.MushroomId,
                        Mushroom = new MushroomDisplayModel(h.Mushroom)
                    });

                    foreach (var item in items)
                    {
                        Mushrooms.Add(item);
                    }

                    HistoryList.ItemsSource = Mushrooms;
                }
            });
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel(new MushroomDisplayModel(((HistoryItemDisplayModel)e.Item).Mushroom));
            await Navigation.PushAsync(new MushroomDetailPage(vm));
        }
    }
}