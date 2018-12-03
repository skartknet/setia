using Setas.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Setas.Services
{
    public interface IInternalDataService
    {
        Task<IEnumerable<MushroomData>> GetMushroomsAsync(SearchOptions options, params int[] ids);
        Task<MushroomData> GetMushroomAsync(int id);
        Task<Configuration> GetConfigurationAsync();
        Task InsertMushroomsAsync(IEnumerable<MushroomData> sourceItems);
        Task SetContentUpdatedAsync();
    }
}
