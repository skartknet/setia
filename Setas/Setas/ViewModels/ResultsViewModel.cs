using Setas.Models;

using System.ComponentModel;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    class ResultsViewModel : INotifyPropertyChanged
    {
     


        Prediction _firstResult;
        public Prediction FirstResult
        {
            get => _firstResult;
            set
            {
                if (_firstResult != value)
                {
                    _firstResult = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FirstResult"));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SecondaryResults"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
