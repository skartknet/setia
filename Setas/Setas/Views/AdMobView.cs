using Xamarin.Forms;

namespace Setas.Controls
{
    public class AdMobView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
               nameof(AdUnitId),
               typeof(string),
               typeof(AdMobView),
               string.Empty);

        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }


        public static readonly BindableProperty AdSizeProperty = BindableProperty.Create(
             nameof(AdSize),
               typeof(string),
               typeof(AdMobView),
               string.Empty
            );

        public string AdSize
        {
            get => (string)GetValue(AdSizeProperty);
            set => SetValue(AdSizeProperty, value);
        }



    }
}