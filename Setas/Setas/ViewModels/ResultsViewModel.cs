using Setas.Common.Models;
using Setas.Models;
using Setas.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public bool FirstResultExists { get
            {
                return FirstResult != null;
            }
        }
        public bool SecondaryResultsExist
        {
            get
            {
                return SecondaryResults != null && SecondaryResults.Any();
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

            SecondaryResults = Enumerable.Empty<Prediction>();
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
                    OnPropertyChanged("FirstResultExists");
                }
            }
        }

        IEnumerable<Prediction> _secondaryResults;
        public IEnumerable<Prediction> SecondaryResults
        {
            get => _secondaryResults;
            set
            {
                _secondaryResults = value;
                OnPropertyChanged("SecondaryResults");
                OnPropertyChanged("SecondaryResultsExist");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
