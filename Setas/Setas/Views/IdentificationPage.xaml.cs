using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Setas
{
    public partial class IdentificationPage : ContentPage
    {
        public const double Miborder = 3.5;
     
        public IdentificationPage()
        {         
            InitializeComponent();

       
        }
        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                ((Button)sender).Text = "Analizando...";
     
                var fileStream = photo.GetStream();

                PredictionResponse result = null;

                try
                {

                    result = await PredictionService.Analyse(StreamToBytes(fileStream));
                }
                catch(Exception ex)
                {
                    throw new Exception("Error getting prediciton", ex);
                }


          

                App.SourceImage = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });

                ((Button)sender).Text = "Hacer foto";

                var vm = new ResultsViewModel
                {
                    FirstResult = result.Predictions.First(),
                    SecondaryResults = result.Predictions.Skip(1).ToArray()
                };

                await Navigation.PushAsync(new IdentificationResultsPage(vm));
            }
        }

        private byte[] StreamToBytes(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            var byteData = binaryReader.ReadBytes((int)stream.Length);
            return byteData;
        }

    }
}