using Setas.Common.Enums;
using Setas.Common.Models;
using Setas.Common.Models.Headless;
using Setas.Services.Headless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Headless.Client.Net.QueryBuilder;

namespace Setas.Services
{
    class HeadlessExternalDataService : IExternalDataService
    {

        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync(DateTime modified)
        {
            var query = Query.Where.UpdateDate.GreaterThanOrEqual(modified);
            try
            {
                var content = await HeadlessClient.Instance.Query<Mushroom>(query);
                return content.Select(m => new MushroomData()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Order = m.Order,
                    PopularNames = m.PopularNames,
                    Season = m.Season,
                    Class = m.Class,
                    Subclass = m.Subclass,
                    Synonyms = m.Synonyms,
                    Confusion = m.Confusion,
                    CookingInstructions = m.CookingInstructions,
                    CookingInterest = (Edible)Enum.Parse(typeof(Edible), m.CookingInterest),
                    Description = m.Description,
                    Family = m.Family,
                    Habitat = m.Habitat,
                    Image = m.Images[0]
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }


           
        }

        public async Task<IEnumerable<MushroomData>> GetAllMushroomsAsync()
        {
            return await this.GetMushroomsAsync(DateTime.MinValue);
        }
    }
}
