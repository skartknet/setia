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
     
        private IdentificationViewModel _vm { get; }
     
        public IdentificationPage(IdentificationViewModel vm)
        {         
            _vm = vm;
            InitializeComponent();

            BindingContext = vm;
        }

    }
}