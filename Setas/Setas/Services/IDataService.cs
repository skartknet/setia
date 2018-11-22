using Setas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Setas.Services
{
    public interface IDataService
    {        

        Task<IEnumerable<Mushroom>> GetMushroomsAsync(params int[] ids);        

        Task<Mushroom> GetMushroomAsync(int id);

        Task InsertMushroomsAsync(IEnumerable<Mushroom> sourceItems);
    }
}
