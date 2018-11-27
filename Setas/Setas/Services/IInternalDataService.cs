using Setas.Common.Models;
using Setas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Setas.Services
{
    public interface IInternalDataService
    {        

        Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options, params int[] ids);        

        Task<Mushroom> GetMushroomAsync(int id);
        Task<Configuration> GetConfigurationAsync();

        Task InsertMushroomsAsync(IEnumerable<Mushroom> sourceItems);

    }
}
