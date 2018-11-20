using Setas.Models;
using Setas.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Setas.ViewModels
{
    public class MushroomsListingViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Mushroom> Mushrooms { get; set; }
        private IDataService _dataService { get; }

        public event PropertyChangedEventHandler PropertyChanged;



        public MushroomsListingViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void GetListingAsync()
        {
            try
            {
                var data = Task.Run(async () => await _dataService.GetMushroomsAsync()).Result;
                Mushrooms = new ObservableCollection<Mushroom>(data);
                PropertyChanged(this, new PropertyChangedEventArgs("Mushrooms"));
            }
            catch (Exception ex) {

            }
        }
    }
}
