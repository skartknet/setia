using Newtonsoft.Json;
using Setas.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Setas.Services
{

    public class ExternalDataService : IExternalDataService
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
                    if (modifiedSince == DateTime.MinValue)
                    {
                        response = await client.GetAsync(uri);
                    }
                    else
                    {
                        response = await client.GetAsync(uri + "?modifiedSince=" + modifiedSince);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        items = JsonConvert.DeserializeObject<List<MushroomData>>(content);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return items;
            }
        }

        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync()
        {
            return await this.GetMushroomsAsync(DateTime.MinValue);
        }



        public async Task<Configuration> GetConfigurationAsync()
        {
            Configuration configuration = null;
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 10);


                var uri = new Uri(baseUrl, "setas/GetConfiguration");
                try
                {
                    var response = await client.GetAsync(uri);


                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        configuration = JsonConvert.DeserializeObject<Configuration>(content);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return configuration;
        }
    }
}
