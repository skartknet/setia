using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Setas.Enums;
using Setas.ViewModels.Helpers;
using Setas.Views;

namespace Setas.ViewModels
{
    class CatalogueMasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CatalogueMenuItem> MenuItems { get; set; }

        public CatalogueMasterViewModel()
        {
            MenuItems = new ObservableCollection<CatalogueMenuItem>(new[]
            {
                    new CatalogueMenuItem { Value = null, Title = "Todas" },
                    new CatalogueMenuItem { Value = EdibleTopClass.Safe, Title = EdibleHelpers.EdibleTopClassToReadableString(EdibleTopClass.Safe)},
                    new CatalogueMenuItem { Value = EdibleTopClass.Toxic, Title = EdibleHelpers.EdibleTopClassToReadableString(EdibleTopClass.Toxic) },
                    new CatalogueMenuItem { Value = EdibleTopClass.Warning, Title = EdibleHelpers.EdibleTopClassToReadableString(EdibleTopClass.Warning) },
                    new CatalogueMenuItem { Value = EdibleTopClass.NoInterest, Title = EdibleHelpers.EdibleTopClassToReadableString(EdibleTopClass.NoInterest) },
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
