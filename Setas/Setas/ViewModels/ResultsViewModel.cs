using Setas.Common.Models;
using Setas.Models;
using Setas.Views;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    class ResultsViewModel : INotifyPropertyChanged
    {
        public ICommand NavToItemDetailsCommand { get; set; }

        public INavigation Navigation { get; set; }


        public ResultsViewModel()
        {
            NavToItemDetailsCommand = new Command<Mushroom>(NavToItemDetails);
        }

        void NavToItemDetails(Mushroom mushroom)
        {
            var model = new MushroomDetailViewModel(mushroom);
            Navigation.PushAsync(new MushroomDetail(model));
        }


        Prediction _firstResult;
        public Prediction FirstResult
        {
            get => _firstResult;
            set
            {
                if (_firstResult != value)
                {
                    _firstResult = value;
                    OnPropertyChanged("FirstResult");
                }
            }
        }

        Prediction[] _secondaryResults;
        public Prediction[] SecondaryResults
        {
            get => _secondaryResults;
            set
            {
                _secondaryResults = value;
                OnPropertyChanged("SecondaryResults");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
