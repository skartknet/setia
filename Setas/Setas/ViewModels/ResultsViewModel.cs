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

        public string AdUnitId
        {
            get
            {
                return App.AdUnitId;
            }
        }

        public bool IsOverProbabilityThreshold { get
            {
                return FirstResult != null ? FirstResult.Probability * 100 > App.ProbabilityThresholdFirstResult :  false;
            }
        }

        public ResultsViewModel()
        {
            NavToItemDetailsCommand = new Command<MushroomDisplayModel>(NavToItemDetails);
        }

        void NavToItemDetails(MushroomDisplayModel mushroom)
        {
            var model = new MushroomDetailViewModel(mushroom);
            Navigation.PushAsync(new MushroomDetailPage(model));
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
                    OnPropertyChanged("IsOverProbabilityThreshold");
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
