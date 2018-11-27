using Setas.Common.Models;
using Setas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Setas.Services
{
    public interface IExternalDataService
    {        

        Task<IEnumerable<Mushroom>> GetMushroomsAsync();

        Task<Configuration> GetConfigurationAsync();
    }
}
