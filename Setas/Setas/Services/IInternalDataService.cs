using Setas.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Setas.Models.Data;
using System;
using Setas.Data;

namespace Setas.Services
{
    public interface IInternalDataService
    {
        bool DatabaseInitialized { get; }

        Task Initialise();

        Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options);
        Task<IEnumerable<Mushroom>> GetMushroomsAsync(IEnumerable<int> ids);

        Task<int> GetTotalCountAsync(SearchOptions options);

        Task<Mushroom> GetMushroomAsync(int id);
        Task<Configuration> GetConfigurationAsync();
        Task InsertMushroomsAsync(IEnumerable<Mushroom> sourceItems);

        Task<IEnumerable<HistoryItem>> GetHistoryAsync();

        /// <summary>
        /// Sets the content updated date to Now
        /// </summary>
        /// <returns></returns>
        Task SetContentUpdatedAsync();

        Task SaveHistoryItemAsync(HistoryItem item);
    }
}
