using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AppCenter.Crashes;
using Setas.Common.Models;
using Setas.Data;
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
            var sourceItems = await _remoteStorage.GetMushroomsAsync();

            var data = Mapper.Map<IEnumerable<Mushroom>>(sourceItems);


            await _localStorage.InsertMushroomsAsync(data);
            await _localStorage.SetContentUpdatedAsync();

        }

        private async Task UpdateContent()
        {
            //if the sync period hasn't been reached we don't sync.
            if (_localConfig.LatestContentUpdate.Value.Add(App.SyncPeriod) > DateTime.UtcNow) return;

            var updateSince = _localConfig.LatestContentUpdate.HasValue ? _localConfig.LatestContentUpdate.Value :
                DateTime.MinValue;


            //Internal DB out of date
            var sourceItems = await _remoteStorage.GetMushroomsAsync(updateSince);

            var data = Mapper.Map<IEnumerable<Mushroom>>(sourceItems);

            await _localStorage.InsertMushroomsAsync(data);
            await _localStorage.SetContentUpdatedAsync();

        }
    }
}
