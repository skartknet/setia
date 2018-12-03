using Microsoft.AppCenter.Crashes;
using Setas.Common.Models;
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
            Configuration intConfiguration = null;
            try
            {
                intConfiguration = await _target.GetConfigurationAsync();
            }
            catch
            { }

            _lastContentSync = intConfiguration?.LatestContentUpdate;

            if (_lastContentSync == null || !_lastContentSync.HasValue)
            {
                try
                {
                    await InitContent();
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
                }
                catch { Crashes.TrackError(new Exception("Error getting external configuration")); }
            }
        }


        private async Task InitContent()
        {

            var sourceItems = await _source.GetMushroomsAsync();
            await _target.InsertMushroomsAsync(sourceItems);

        }

        private async Task UpdateContent()
        {
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

            //we update
            if (_lastContentUpdate > _lastContentSync)
            {
                //Internal DB out of date

                try
                {
                    var sourceItems = await _source.GetMushroomsAsync();
                    await _target.InsertMushroomsAsync(sourceItems);
                    await _target.SetContentUpdatedAsync();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex, new Dictionary<string, string> { { "stage", "updating db" } });
                }
            }
        }
    }
}
