using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Setas.Views
{
    public class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Cargando..." }
                }
            };
        }
    }
}