using Setas.Models;

using System.ComponentModel;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    class ResultsViewModel : INotifyPropertyChanged
    {

        public ResultsViewModel()
        {

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
