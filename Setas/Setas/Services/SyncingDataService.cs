using System.Threading.Tasks;

namespace Setas.Services
{
    class SyncingDataService : ISyncingDataService
    {
        private IDataService _source { get; }
        private IDataService _target { get; }


        public SyncingDataService(IDataService source, IDataService target)
        {
            _source = source;
            _target = target;
        }

        /// <summary>
        /// Syncs internal data with external API
        /// </summary>
        public async Task SyncDataAsync()
        {
            var sourceItems = await _source.GetMushroomsAsync();

            await _target.InsertMushroomsAsync(sourceItems);
        }

       
    }
}
