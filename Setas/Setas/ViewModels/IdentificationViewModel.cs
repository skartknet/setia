using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Setas.Common.Models;
using Setas.Models;
using Setas.Services;
using Setas.Helpers;
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
            catch (Exception ex)
            {
                throw ex;
            }



        }

        private async Task IdentifyImage(MediaFile image)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
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
                    result = await _predictionService.PredictImageAsync(App.CustomVisionProjectId, fileStream);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await UserDialogs.Instance.AlertAsync("Error analizando imagen. No se pudo conectar con el servicio.", "Error");

                    return;
                }

                try
                {
                    var vm = await CreateResultViewModel(result);

                    await Navigation.PushAsync(new IdentificationResultsPage(vm));

                }
                catch { return; }

            }

        }

        private async System.Threading.Tasks.Task<ResultsViewModel> CreateResultViewModel(ImagePrediction result)
        {

            var vm = new ResultsViewModel();

            try
            {
                if (!result.Predictions.Any()) return null;

                var resultsToDisplay = await MapToPredictionDisplay(result.Predictions);

                vm.FirstResult = resultsToDisplay.FirstOrDefault(m => m.Probability >= App.ProbabilityThreshold);


                if (vm.FirstResult != null)
                {
                    resultsToDisplay.Remove(vm.FirstResult);

                    await _dataService.SaveHistoryItemAsync(new Models.Data.HistoryItem()
                    {
                        TakenOn = DateTime.Now,
                        MushroomId = vm.FirstResult.Mushroom.Id
                    });
                }                                                


                vm.SecondaryResults = resultsToDisplay.ToArray();

            }
            catch (WebException ex)
            {
                Crashes.TrackError(ex);
                await UserDialogs.Instance.AlertAsync("Error conectando a servicio de datos.", "Error");
                throw;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await UserDialogs.Instance.AlertAsync("Error de datos.", "Error");
                throw;

            }

            return vm;

        }

        private async Task<IList<Prediction>> MapToPredictionDisplay(IList<PredictionModel> models)
        {
            var rIds = models.Select(r => r.CmsNodeId()).ToArray();
            var resultToDisplay = await _dataService.GetMushroomsAsync(rIds);

            var resultsViewModel = new List<Prediction>();

            foreach (var model in models)
            {
                var viewModel = new Prediction
                {
                    Mushroom = new MushroomDisplayModel(resultToDisplay.FirstOrDefault(m => m.Id == model.CmsNodeId())),
                    Probability = model.Probability
                };

                resultsViewModel.Add(viewModel);
            }

            return resultsViewModel.OrderByDescending(n=>n.Probability).ToList();
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
