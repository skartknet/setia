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
                if (Device.RuntimePlatform == Device.iOS)
                    return "ca-app-pub-2003726790886919/4839520685";
                else if (Device.RuntimePlatform == Device.Android)
                    return "ca-app-pub-2003726790886919/3499977722";
                else return null;
            }
        }

        public bool IsOverProbabilityThreshold { get
            {
                return FirstResult != null ? FirstResult.Probability > App.ProbabilityThreshold :  false;
            }
        }

        public ResultsViewModel()
        {
            NavToItemDetailsCommand = new Command<MushroomDisplayModel>(NavToItemDetails);
        }

        void NavToItemDetails(MushroomDisplayModel mushroom)
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
