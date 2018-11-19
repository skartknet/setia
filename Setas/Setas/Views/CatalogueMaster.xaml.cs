using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                    new CatalogueMenuItem { Id = 0, Title = "Page 1" },
                    new CatalogueMenuItem { Id = 1, Title = "Page 2" },
                    new CatalogueMenuItem { Id = 2, Title = "Page 3" },
                    new CatalogueMenuItem { Id = 3, Title = "Page 4" },
                    new CatalogueMenuItem { Id = 4, Title = "Page 5" },
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

        private void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new CatalogueDetail());
        }
    }
}