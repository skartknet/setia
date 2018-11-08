using Setas.Models;
using System.ComponentModel;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    class ResultsViewModel : INotifyPropertyChanged
    {
        public ResultsViewModel()
        {

            if (DesignMode.IsDesignModeEnabled)
            {

                FirstResult = new Prediction
                {
                    TagName = "Seta 1",
                    Probability = 55.5f
                };
                SecondaryResults = new Prediction[]
                {
                    new Prediction
                    {
                        TagName ="Seta 2",
                        Probability = 55.5f
                    },
                    new Prediction
                    {
                        TagName ="Seta 3",
                        Probability = 38f
                    },
                    new Prediction
                    {
                        TagName ="Seta 4",
                        Probability = 10f
                    }
                };


                Setas = new Seta[]
                {
                    new Seta
                    {
                        Name="Seta 1",
                        Image = "http://placehold.it/300x300"
                    },
                    new Seta
                    {
                        Name="Seta 2",
                        Image = "http://placehold.it/300x300"
                    },
                    new Seta
                    {
                        Name="Seta 3",
                        Image = "http://placehold.it/300x300"
                    },
                    new Seta
                    {
                        Name="Seta 4",
                        Image = "http://placehold.it/300x300"
                    },
                };
            }

        }


        Seta[] _setas;
        public Seta[] Setas
        {
            get => _setas;
            set
            {
                _setas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Setas"));
            }
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
