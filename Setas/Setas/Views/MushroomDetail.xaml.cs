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
	public partial class MushroomDetail : ContentPage
	{

        public MushroomDetail()
		{
			InitializeComponent ();
		}

        public MushroomDetail(MushroomDetailViewModel model) : this()
        {            
            BindingContext = model;
        }
    }
}