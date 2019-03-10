using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Setas.Common.Models.Api;

namespace Setas.Services
{
    public interface IExternalDataService
    {

        Task<IEnumerable<Mushroom>> GetMushroomsAsync(DateTime modifiedSince);
        Task<IEnumerable<Mushroom>> GetMushroomsAsync();
    }
}
