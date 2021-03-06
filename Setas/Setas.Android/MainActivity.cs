using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;

namespace Setas.Droid
{
    [Activity(Label = "Setia", Icon = "@drawable/setia_logo", Theme = "@style/splashscreen", MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            UserDialogs.Init(this);

            base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-2003726790886919~4164647540");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);

#if GORILLA
            LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(
                this, new UXDivers.Gorilla.Config("Good Gorilla")
           ));
#else
            LoadApplication(new App());

#endif




        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}