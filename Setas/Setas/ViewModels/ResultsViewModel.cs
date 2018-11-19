using Setas.Models;

using System.ComponentModel;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    class ResultsViewModel : INotifyPropertyChanged
    {
     


        Mushroom _firstResult;
        public Mushroom FirstResult
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

        Mushroom[] _secondaryResults;
        public Mushroom[] SecondaryResults
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
