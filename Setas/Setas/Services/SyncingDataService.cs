using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Setas.Services
{
    class SyncingDataService : ISyncingDataService
    {
        private IExternalDataService _source { get; }
        private IInternalDataService _target { get; }

        //stores the date time when the external db was updated.
        private DateTime? _lastContentUpdate;

        //stores the date time when the internal db was updated.
        private DateTime? _lastContentSync;

        public SyncingDataService(IExternalDataService source, IInternalDataService target)
        {
            _source = source;
            _target = target;
        }

        /// <summary>
        /// Syncs internal data with external API
        /// </summary>
        public async Task SyncDataAsync()
        {
            var intConfiguration = await _target.GetConfigurationAsync();
            _lastContentSync = intConfiguration?.LatestContentUpdate;

            if (!_lastContentSync.HasValue)
            {
                //First run of App, DB doesn't exist

                bool didAppCrash = await Crashes.HasCrashedInLastSessionAsync();

                if (didAppCrash)
                {
                    await UserDialogs.Instance.AlertAsync("Parece que hubo un error creando la base de datos. Vamos a intentar otra vez....");
                }

                try
                {
                    var sourceItems = await _source.GetMushroomsAsync();
                    await _target.InsertMushroomsAsync(sourceItems);
                }
                catch (Exception ex)
                {
                    //we can't run the App without data
                    await UserDialogs.Instance.AlertAsync("No se pudo descargar el contenido por primera vez. La app no se puede ejecutar.");
                    Crashes.TrackError(ex, new Dictionary<string, string> { { "stage", "first run" } });

                    throw new Exception(); //to terminate app
                }
            }

            try
            {
                //get external configuration
                var extConfiguration = await _source.GetConfigurationAsync();

                if (extConfiguration != null)
                {
                    _lastContentUpdate = extConfiguration.LatestContentUpdate;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(new Exception("Error getting external configuration", ex));
            }

            if (_lastContentUpdate > _lastContentSync)
            {
                //Internal DB out of date

                try
                {
                    var sourceItems = await _source.GetMushroomsAsync();
                    await _target.InsertMushroomsAsync(sourceItems);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex, new Dictionary<string, string> { { "stage", "updating db" } });
                }
            }
        }


    }
}
