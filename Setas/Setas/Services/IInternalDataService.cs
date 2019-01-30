using Setas.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Setas.Models.Data;
using System;

namespace Setas.Services
{
    public interface IInternalDataService
    {
        Task<IEnumerable<MushroomData>> GetMushroomsAsync(SearchOptions options, params Guid[] ids);
        Task<MushroomData> GetMushroomAsync(Guid id);
        Task<Configuration> GetConfigurationAsync();
        Task InsertMushroomsAsync(IEnumerable<MushroomData> sourceItems);

        Task<IEnumerable<HistoryItem>> GetHistoryAsync();

        /// <summary>
        /// Sets the content updated date to Now
        /// </summary>
        /// <returns></returns>
        Task SetContentUpdatedAsync();

        Task SaveHistoryItemAsync(HistoryItem item);
    }
}
