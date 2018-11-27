using Setas.Common.Models;
using System.ComponentModel;

namespace Setas.ViewModels
{
    public class MushroomDetailViewModel : INotifyPropertyChanged
    {
        public Mushroom Mushroom { get; set; }

        public MushroomDetailViewModel()
        {

        }

        public MushroomDetailViewModel(Mushroom item)
        {
            Mushroom = item;
        }
  

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
