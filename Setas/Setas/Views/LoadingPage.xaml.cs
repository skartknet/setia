using Acr.UserDialogs;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Setas
{
    public partial class LoadingPage : ContentPage
    {                  
        public LoadingPage()
        {         
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading("Cargando...");            
        }
    }
}