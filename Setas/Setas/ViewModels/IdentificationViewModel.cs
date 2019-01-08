using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
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

        private IPredictionService _predictionService;

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

        public IdentificationViewModel(IInternalDataService dataService, IPredictionService predictionService)
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

                PredictionResponse result = null;

                try
                {

                    result = await _predictionService.Analyse(StreamToBytes(fileStream));
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await UserDialogs.Instance.AlertAsync("Error analizando imagen. No se pudo conectar con el servicio.", "Error");
                }



                App.SourceImage = ImageSource.FromStream(() =>
                {
                    return image.GetStream();
                });



                var vm = await CreateResultViewModel(result);

                if (vm != null)
                {
                    await Navigation.PushAsync(new IdentificationResultsPage(vm));
                }

            }

        }

        private async System.Threading.Tasks.Task<ResultsViewModel> CreateResultViewModel(PredictionResponse result)
        {

            var vm = new ResultsViewModel();

            try
            {
                var firstResultPrediction = result.Predictions.FirstOrDefault(m=>m.Probability >= App.ProbabilityThreshold);

                IEnumerable<Prediction> secondaryResultsPredictions = Enumerable.Empty<Prediction>();

                if (firstResultPrediction != null)
                {
                    var rId = Helpers.Predictions.TagToItemId(firstResultPrediction.TagName);
                    firstResultPrediction.Mushroom = new MushroomDisplayModel(await _dataService.GetMushroomAsync(rId));
                    secondaryResultsPredictions = result.Predictions.Skip(1);

                    var hItem = new Models.Data.HistoryItem()
                    {
                        TakenOn = DateTime.Now,
                        MushroomId = firstResultPrediction.Mushroom.Id
                    };

                    await _dataService.SaveHistoryItemAsync(hItem);
                }
                else
                {
                    secondaryResultsPredictions = result.Predictions;
                }


                var rIds = secondaryResultsPredictions.Select(r => Helpers.Predictions.TagToItemId(r.TagName)).ToArray();
                var secondaryResultsMushrooms = await _dataService.GetMushroomsAsync(new SearchOptions(), rIds);

                foreach (var item in secondaryResultsPredictions)
                {
                    item.Mushroom = new MushroomDisplayModel(secondaryResultsMushrooms.FirstOrDefault(m => m.Id == Helpers.Predictions.TagToItemId(item.TagName)));
                }


                vm.FirstResult = firstResultPrediction;
                vm.SecondaryResults = secondaryResultsPredictions.ToArray();

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
