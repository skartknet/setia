using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Setas.Common.Models;
using Setas.Services.Headless;
using System.Linq;

namespace Setas.Services
{
    class HeadlessExternalDataService : IExternalDataService
    {
        public Task<Configuration> GetConfigurationAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync(DateTime modifiedSince)
        {
            var content = await HeadlessClient.Instance.GetAll("Mushroom");

            return content.Select(m => new MushroomData()
            {
                Id = m.Id,
                Name = m.Name
            });
        }

        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync()
        {
            return await this.GetMushroomsAsync(DateTime.MinValue);           
        }
    }
}
