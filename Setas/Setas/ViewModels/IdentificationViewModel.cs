using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Setas.Common.Models;
using Setas.Models;
using Setas.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Setas.ViewModels
{
    public class IdentificationViewModel : INotifyPropertyChanged
    {

        private IInternalDataService _dataService;

        private ICustomVisionPredictionClient _predictionService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand TakePhoto { get; }

        public ICommand PickPhoto { get; }

        public INavigation Navigation { get; set; }


        

        public string AdUnitId
        {
            get
            {
                return App.AdUnitId;
            }
        }

        public bool IsTakePhotoSupported
        {
            get
            {
                return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
            }
        }
        public bool IsPickPhotoSupported
        {
            get
            {
                return CrossMedia.Current.IsPickPhotoSupported;
            }
        }


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
                    OnPropertyChanged("IsNotIdentifying");
                }
            }
        }

        public bool IsNotIdentifying
        {
            get
            {
                return !IsIdentifying;
            }
        }

        public IdentificationViewModel()
        { }

        public IdentificationViewModel(IInternalDataService dataService, ICustomVisionPredictionClient predictionService)
        {
            _dataService = dataService;
            _predictionService = predictionService;


            TakePhoto = new Command(TakePhotoProcess);
            PickPhoto = new Command(PickPhotoProcess);
        }

        private async void TakePhotoProcess(object obj)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 1000,
                CompressionQuality = 80,
                AllowCropping = true,
                Directory = "Setia",
                SaveToAlbum = true
            });

            if (photo != null)
            {
                await IdentifyImage(photo);
            }
        }

        private async void PickPhotoProcess(object obj)
        {
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 1000,
                    CompressionQuality = 80
                });

                if (photo != null)
                {
                    await IdentifyImage(photo);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
                

            
        }

        private async Task IdentifyImage(MediaFile image)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None) {
                UserDialogs.Instance.Alert("Se necesita conexión a la red para realizar esta operación.", "Desconectado", "Aceptar");
                return;
            };



            //TODO: Apply permissions recommendations: https://github.com/jamesmontemagno/MediaPlugin#permission-recommendations
            //TODO: return null exception and pick it up for AppCenter and display Userdialog (https://github.com/aritchie/userdialogs/blob/master/docs/progress.md)

            if (image == null) return;

            using (UserDialogs.Instance.Loading("Analizando..."))
            {
                var fileStream = image.GetStream();

                App.SourceImage = ImageSource.FromStream(() =>
                {
                    return image.GetStream();
                });



                ImagePrediction result = null;

                try
                {

                    result = await _predictionService.PredictImageAsync(App.CustomVisionProjectKey, fileStream);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await UserDialogs.Instance.AlertAsync("Error analizando imagen. No se pudo conectar con el servicio.", "Error");
                }



               



                var vm = await CreateResultViewModel(result);

                if (vm != null)
                {
                    await Navigation.PushAsync(new IdentificationResultsPage(vm));
                }

            }

        }

        private async System.Threading.Tasks.Task<ResultsViewModel> CreateResultViewModel(ImagePrediction result)
        {

            var vm = new ResultsViewModel();

            try
            {
                if (!result.Predictions.Any()) return null;

                var firstResultPrediction = result.Predictions.FirstOrDefault(m=>m.Probability >= App.ProbabilityThreshold);

                IEnumerable<PredictionModel> secondaryResultsPredictions = Enumerable.Empty<PredictionModel>();                

                if (firstResultPrediction != null)
                {
                    var firstResultViewModel = new Prediction();
                    var rId = Helpers.Predictions.TagToItemId(firstResultPrediction.TagName);

                    firstResultViewModel.Mushroom = new MushroomDisplayModel(await _dataService.GetMushroomAsync(rId));
                    firstResultViewModel.Probability = firstResultPrediction.Probability;
                    vm.FirstResult = firstResultViewModel;

                    secondaryResultsPredictions = result.Predictions.Skip(1);


                    var hItem = new Models.Data.HistoryItem()
                    {
                        TakenOn = DateTime.Now,
                        MushroomId = firstResultViewModel.Mushroom.Id
                    };



                    await _dataService.SaveHistoryItemAsync(hItem);
                }
                else
                {
                    secondaryResultsPredictions = result.Predictions;
                }


                var rIds = secondaryResultsPredictions.Select(r => Helpers.Predictions.TagToItemId(r.TagName)).ToArray();
                var secondaryResultsMushrooms = await _dataService.GetMushroomsAsync(new SearchOptions(), rIds);

                var secResultsViewModel = new List<Prediction>();

                foreach (var item in secondaryResultsPredictions)
                {
                    var viewModel = new Prediction
                    {
                        Mushroom = new MushroomDisplayModel(secondaryResultsMushrooms.FirstOrDefault(m => m.Id == Helpers.Predictions.TagToItemId(item.TagName))),
                       Probability = item.Probability
                    };

                    secResultsViewModel.Add(viewModel);
                }


                vm.SecondaryResults = secResultsViewModel.ToArray();

            }
            catch (WebException ex)
            {
                Crashes.TrackError(ex);
                await UserDialogs.Instance.AlertAsync("Error conectando a servicio de datos.", "Error");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await UserDialogs.Instance.AlertAsync("Error de datos.", "Error");
            }

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
