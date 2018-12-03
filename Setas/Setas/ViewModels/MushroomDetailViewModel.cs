using Setas.Common.Models;
using Setas.Models;
using System.ComponentModel;

namespace Setas.ViewModels
{
    public class MushroomDetailViewModel : INotifyPropertyChanged
    {
        public MushroomDisplayModel Mushroom { get; set; }

        public MushroomDetailViewModel()
        {

        }

        public MushroomDetailViewModel(MushroomDisplayModel item)
        {
            Mushroom = item;
        }
  

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
