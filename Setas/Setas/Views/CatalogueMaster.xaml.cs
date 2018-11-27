using Setas.Common.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Setas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogueMaster : ContentPage
    {
        public ListView ListView;

        public CatalogueMaster()
        {
            InitializeComponent();

            BindingContext = new CatalogueMasterViewModel();
            ListView = MenuItemsListView;
        }

        class CatalogueMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<CatalogueMenuItem> MenuItems { get; set; }

            public CatalogueMasterViewModel()
            {
                MenuItems = new ObservableCollection<CatalogueMenuItem>(new[]
                {
                    new CatalogueMenuItem { Id = Edible.BuenComestible, Title = "Buen Comestible" },
                    new CatalogueMenuItem { Id = Edible.Toxica, Title = "Tóxica" },
                    new CatalogueMenuItem { Id = Edible.SinInteres, Title = "Sin Interés" },
                    new CatalogueMenuItem { Id = Edible.ComestibleConPrecaucion, Title = "Comestible Con Precaución" },
                    new CatalogueMenuItem { Id = Edible.ComestiblePeroPeligrosa, Title = "Comestible Pero Peligrosa" },
                    new CatalogueMenuItem { Id = Edible.ComestibleCalidadMedia, Title = "Comestible Calidad Media" },
                    new CatalogueMenuItem { Id = Edible.ComestibleBajaCalidad, Title = "Comestible Baja Calidad" },
                    new CatalogueMenuItem { Id = Edible.PosibleToxico, Title = "Posible Tóxico" },
                    new CatalogueMenuItem { Id = Edible.NoComestible, Title = "No Comestible" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }


    }
}