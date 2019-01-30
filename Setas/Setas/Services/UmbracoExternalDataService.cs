using Newtonsoft.Json;
using Setas.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Setas.Services
{

    public class UmbracoExternalDataService : IExternalDataService
    {

        readonly Uri baseUrl = new Uri(App.ExternalService, App.ApiBase);


        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync(DateTime modifiedSince)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(baseUrl, "setas/GetMushrooms");
                var items = Enumerable.Empty<MushroomData>();

                try
                {
                    HttpResponseMessage response;

                    response = await client.GetAsync(uri + "?modifiedSince=" + modifiedSince);


                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        items = JsonConvert.DeserializeObject<List<MushroomData>>(content);
                    }
                    else
                    {
                        throw new Exception("Couldn't query external server.");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return items;
            }
        }

        public async Task<IEnumerable<MushroomData>> GetAllMushroomsAsync()
        {
            return await this.GetMushroomsAsync(DateTime.MinValue);
        }
        
    }
}
