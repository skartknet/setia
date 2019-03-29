using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Setas.Models;
using Setas.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MushroomDetailPage : ContentPage
	{

        public MushroomDetailPage()
		{
			InitializeComponent ();
		}

        public MushroomDetailPage(MushroomDetailViewModel model) : this()
        {            
            BindingContext = model;

            var licenseWeb = new HtmlWebViewSource
            {
                Html = model.Mushroom.ImageLicense
            };

            licenseWebView.Source = licenseWeb;
            licenseWebView.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith("http"))
                {
                    try
                    {
                        var uri = new Uri(e.Url);
                        Device.OpenUri(uri);
                    }
                    catch (Exception)
                    {
                    }

                    e.Cancel = true;
                }
            };
        }
    }
}