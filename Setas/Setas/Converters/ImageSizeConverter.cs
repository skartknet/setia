using Setas.Images;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Setas.Converters
{
    public class ImageSizeConverter : IValueConverter
    {

        public string ImageUrl { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var size = (string)parameter;

            var image = Xamarin.Forms.ImageSource.FromUri(new Uri(App.ExternalService, ImageUrl));
            return image;


            //var imageSize = (ImagesSizes)Enum.Parse(typeof(ImagesSizes), (string)value);

            //return ImagesSizesDictionary.GetValue(imageSize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
