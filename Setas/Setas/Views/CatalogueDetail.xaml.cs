using Setas.Common.Enums;
using Setas.Common.Models;
using Setas.Services;
using Setas.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueDetail : ContentPage
    {        

        public IEnumerable<Mushroom> Mushrooms;

        public IInternalDataService _dataService { get; }

        public CatalogueDetail()
        {
            InitializeComponent();



        }

        public CatalogueDetail(IInternalDataService dataService, Edible? edible)
        {
            _dataService = dataService;

            Task.Run(async () =>
            {
                Mushrooms = await _dataService.GetMushroomsAsync(new SearchOptions
                {
                    Edible = edible
                });

                DetailsList.ItemsSource = Mushrooms;
            });


        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = new MushroomDetailViewModel((Mushroom)e.Item);
            Navigation.PushAsync(new MushroomDetail(vm));
        }
    }
}