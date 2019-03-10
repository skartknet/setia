using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Setas.Common.Models;
using Setas.Services.Headless;
using Umbraco.Headless.Client.Net.QueryBuilder;

namespace Setas.Services
{
    class HeadlessExternalDataService : IExternalDataService
    {

        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync(DateTime modifiedSince)
        {
            try
            {
                var query = Query.Where.UpdateDate.GreaterThan(modifiedSince).


                var content = await HeadlessClient.Instance.Query<MushroomData>("Mushroom");

                return content.Select(m => new MushroomData()
                {
                    Id = m.Id,
                    Name = m.Name
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync()
        {
            return await this.GetMushroomsAsync(DateTime.MinValue);
        }
    }
}
