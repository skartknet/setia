
using Xamarin.Forms;

namespace Setas.Views
{
    public class SetasNavigationPage : NavigationPage
    {
        public SetasNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = Color.DarkBlue;
            BarTextColor = Color.White;
        }
    }
}