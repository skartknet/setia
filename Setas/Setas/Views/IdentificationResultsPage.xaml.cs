using Setas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentificationResultsPage : ContentPage
	{

        ResultsViewModel vm;

		internal IdentificationResultsPage (ResultsViewModel _vm)
		{
			InitializeComponent ();

            vm = _vm;
            BindingContext = vm;

		}
	}
}