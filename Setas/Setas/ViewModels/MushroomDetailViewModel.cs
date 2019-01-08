using Setas.Common.Models;
using Setas.Models;
using System.ComponentModel;

namespace Setas.ViewModels
{
    public class MushroomDetailViewModel
    {
        public MushroomDisplayModel Mushroom { get; set; }

        public string AdUnitId
        {
            get
            {
                return App.AdUnitId;
            }
        }


        public MushroomDetailViewModel()
        {

        }

        public MushroomDetailViewModel(MushroomDisplayModel item)
        {
            Mushroom = item;
        }
          
    }
}
