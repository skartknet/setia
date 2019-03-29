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


            TakePhoto = new Command(async (obj)=> await TakePhotoProcess(obj));
            PickPhoto = new Command(async (obj)=> await PickPhotoProcess(obj));
        }

        private async Task TakePhotoProcess(object obj)
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
                await IdentifyImageAsync(photo);
            }
        }

        private async Task PickPhotoProcess(object obj)
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
                    await IdentifyImageAsync(photo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        private async Task IdentifyImageAsync(MediaFile image)
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

                    result = await _predictionService.ClassifyImageAsync(App.CustomVisionProjectId, "Iteration1", fileStream);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await UserDialogs.Instance.AlertAsync("Error analizando imagen. No se pudo conectar con el servicio.", "Error");

                    return;
                }

                try
                {
                    var vm = await CreateResultViewModelAsync(result);

                    await Navigation.PushAsync(new IdentificationResultsPage(vm));

                }
                catch { return; }

            }

        }

        private async Task<ResultsViewModel> CreateResultViewModelAsync(ImagePrediction result)
        {

            var vm = new ResultsViewModel();

            try
            {
                if (!result.Predictions.Any()) return null;

                var resultsToDisplay = await MapToPredictionDisplayAsync(result.Predictions);

                vm.FirstResult = resultsToDisplay.FirstOrDefault(m => m.Probability * 100 >= App.ProbabilityThresholdFirstResult);


                if (vm.FirstResult != null)
                {
                    resultsToDisplay.Remove(vm.FirstResult);

                    await _dataService.SaveHistoryItemAsync(new Models.Data.HistoryItem()
                    {
                        TakenOn = DateTime.Now,
                        MushroomId = vm.FirstResult.Mushroom.Id
                    });
                }                                                


                vm.SecondaryResults = resultsToDisplay.Where(m=>m.Probability * 100 >= App.ProbabilityThresholdSecondaryResults).Take(5).ToArray();

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

        private async Task<IList<Prediction>> MapToPredictionDisplayAsync(IList<PredictionModel> models)
        {
            var rIds = models.Select(r => r.CmsNodeId()).ToArray();

            //this line will not return thos results that are not in the database.
            var resultToDisplay = await _dataService.GetMushroomsAsync(rIds);

            var resultsViewModel = new List<Prediction>();


            //so here we iterate only over those valid results instead the whole list of predictions
            foreach (var model in resultToDisplay)
            {
                var t = models.FirstOrDefault(m => m.CmsNodeId() == model.Id);
                var viewModel = new Prediction
                {
                    Mushroom = new MushroomDisplayModel(model),
                    Probability = t.Probability
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
