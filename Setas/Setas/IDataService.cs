using Setas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setas
{
    public interface IDataService
    {        

        Task<IEnumerable<Mushroom>> GetMushroomsAsync(params int[] ids);

        Task<Mushroom> GetMushroomAsync(int id);
    }
}
