using Setas.Common.Models;
using Setas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Setas.Services
{
    public interface IExternalDataService
    {        

        Task<IEnumerable<MushroomData>> GetMushroomsAsync();

        Task<Configuration> GetConfigurationAsync();
    }
}
