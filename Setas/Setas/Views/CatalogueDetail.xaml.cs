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

        public ObservableCollection<MushroomDisplayModel> Mushrooms;

        public IInternalDataService _dataService { get; }

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


        public CatalogueDetail(IInternalDataService dataService, Edible[] edibles)
        {
            _dataService = dataService;

            Task.Run(async () =>
            {
                var data = await _dataService.GetMushroomsAsync(new SearchOptions
                {
                    Edibles = edibles
                });

                Mushrooms = new ObservableCollection<MushroomDisplayModel>(data.Select(m => new MushroomDisplayModel(m)));

            }).Wait();

            InitializeComponent();

            DetailsList.ItemsSource = Mushrooms;

        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel((MushroomDisplayModel)e.Item);
            await Navigation.PushAsync(new MushroomDetail(vm));
        }
    }
}