using Microsoft.AppCenter.Crashes;
using Setas.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Setas.Services
{
    class SyncingDataService : ISyncingDataService
    {
        private IExternalDataService _remoteStorage { get; }
        private IInternalDataService _localStorage { get; }

        //TODO: move configData to static App property
        private Configuration _localConfig;

        public SyncingDataService(IExternalDataService source, IInternalDataService target)
        {
            _remoteStorage = source;
            _localStorage = target;
        }

        /// <summary>
        /// Syncs internal data with external API
        /// </summary>
        public async Task SyncDataAsync()
        {

            if (Connectivity.NetworkAccess == NetworkAccess.None) return;

            try
            {
                _localConfig = await _localStorage.GetConfigurationAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(new Exception("Error retrieving configuration.", ex));
            }

            if (_localConfig.LatestContentUpdate == null || !_localConfig.LatestContentUpdate.HasValue)
            {
                try
                {
                    await InitContent();
                    App.StorageInitialized = true;
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex, new Dictionary<string, string> { { "stage", "first run" } });
                    throw ex;
                }
            }
            else
            {
                try
                {
                    await UpdateContent();
                    App.StorageInitialized = true;

                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex, new Dictionary<string, string> { { "stage", "updating content" } });

                }
            }
        }



        private async Task InitContent()
        {
            var sourceItems = await _remoteStorage.GetAllMushroomsAsync();

            if (sourceItems.Any())
            {
                await _localStorage.InsertMushroomsAsync(sourceItems);
                await _localStorage.SetContentUpdatedAsync();
            }

        }

        private async Task UpdateContent()
        {
            //if the sync period hasn't been reached we don't sync.
            if (_localConfig.LatestContentUpdate.Value.Add(App.SyncPeriod) > DateTime.UtcNow) return;

            var sourceItems = await _remoteStorage.GetMushroomsAsync(_localConfig.LatestContentUpdate.Value);
            if (sourceItems.Any())
            {
                await _localStorage.InsertMushroomsAsync(sourceItems);
                await _localStorage.SetContentUpdatedAsync();
            }

        }
    }
}
