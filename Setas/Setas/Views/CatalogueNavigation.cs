
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;

namespace Setas.Views
{
    public class CatalogueNavigation : Xamarin.Forms.NavigationPage
    {

        private readonly int _androidDefaultBarHeight = 250;

        public CatalogueNavigation(Page page)
        {
            On<Android>().SetBarHeight(450);

            Pushed += SetBarHeightOnChildren;
            Popped += SetBarHeightOnRoot;

            PushAsync(page);
        }

        private void SetBarHeightOnChildren(object sender, NavigationEventArgs e)
        {
            if (e.Page is MushroomDetailPage)
                On<Android>().SetBarHeight(_androidDefaultBarHeight);
        }

        private void SetBarHeightOnRoot(object sender, NavigationEventArgs e)
        {
            On<Android>().SetBarHeight(450);
        }
    }
}