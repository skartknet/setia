using Setas.Common.Enums;
using Setas.Enums;
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
                    new CatalogueMenuItem { Value = null, Title = "Todas" },
                    new CatalogueMenuItem { Value = EdibleTopClassEnum.Safe, Title = "Comestibles" },
                    new CatalogueMenuItem { Value = EdibleTopClassEnum.Toxic, Title = "Tóxicas" },
                    new CatalogueMenuItem { Value = EdibleTopClassEnum.Warning, Title = "Precaución" },
                    new CatalogueMenuItem { Value = EdibleTopClassEnum.NoInterest, Title = "Sin Interés" },               
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