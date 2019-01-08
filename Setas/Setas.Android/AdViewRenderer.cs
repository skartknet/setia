using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using Setas.Controls;
using Setas.Droid;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobViewRenderer))]
namespace Setas.Droid
{
    public class AdMobViewRenderer : ViewRenderer<AdMobView, AdView>
    {
        public AdMobViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control == null)
                SetNativeControl(CreateAdView());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AdView.AdUnitId))
                Control.AdUnitId = Element.AdUnitId;

            if (e.PropertyName == nameof(AdView.AdSize))
                Control.AdUnitId = Element.AdSize;
        }

        private AdView CreateAdView()
        {
            var adView = new AdView(Context)
            {
                AdSize = getSizeEnum(Element.AdSize),
                AdUnitId = Element.AdUnitId
            };


            adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;
        }


        private AdSize getSizeEnum(string adSize)
        {

            switch (adSize)
            {
                case "MEDIUM_RECTANGLE":
                    return AdSize.MediumRectangle;
                default:
                    return AdSize.SmartBanner;

            }
        }
    }
}