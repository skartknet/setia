using Setas.Models;
using Setas.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    public class IdentificationViewModel : INotifyPropertyChanged
    {

        private IDataService _dataService;

        private IPredictionService _predictionService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand IdentifyCommand { get; set; }
        public INavigation Navigation { get; set; }


        private bool _isIdentifying;
        public bool IsIdentifying
        {
            get { return _isIdentifying; }
            set
            {
                if (_isIdentifying != value)
                {
                    _isIdentifying = value;
                    OnPropertyChanged("IsIdentifying");
                }
            }
        }

        public IdentificationViewModel()
        {

        }

        public IdentificationViewModel(IDataService dataService, IPredictionService predictionService)
        {
            _dataService = dataService;
            _predictionService = predictionService;

            IdentifyCommand = new Command(IdentificationProcess);
        }

        private async void IdentificationProcess()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                IsIdentifying = true;


                var fileStream = photo.GetStream();

                PredictionResponse result = null;

                try
                {

                    result = await _predictionService.Analyse(StreamToBytes(fileStream));
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting prediction", ex);
                }



                App.SourceImage = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });


                IsIdentifying = false;

                var vm = await CreateResultViewModel(result);
                await Navigation.PushAsync(new IdentificationResultsPage(vm));
            }
        }

        private async System.Threading.Tasks.Task<ResultsViewModel> CreateResultViewModel(PredictionResponse result)
        {
            var firstResultPrediction = result.Predictions.FirstOrDefault();
            firstResultPrediction.Mushroom = await _dataService.GetMushroomAsync(Helpers.Predictions.TagToItemId(firstResultPrediction.TagName));


            var secondaryResultsPredictions = result.Predictions.Skip(1).ToArray();
            var secondaryResultsMushrooms = await _dataService.GetMushroomsAsync(secondaryResultsPredictions.Select(r => Helpers.Predictions.TagToItemId(r.TagName)).ToArray());

            foreach (var item in secondaryResultsPredictions)
            {
                item.Mushroom = secondaryResultsMushrooms.FirstOrDefault(m => m.Id == Helpers.Predictions.TagToItemId(item.TagName));
            }


            var vm = new ResultsViewModel
            {
                FirstResult = firstResultPrediction,
                SecondaryResults = secondaryResultsPredictions.ToArray()
            };

            return vm;
        }

        private byte[] StreamToBytes(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            var byteData = binaryReader.ReadBytes((int)stream.Length);
            return byteData;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
