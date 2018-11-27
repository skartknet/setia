using Newtonsoft.Json;
using Setas.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Setas.Services
{

    public class ExternalDataService : IExternalDataService
    {

        readonly Uri baseUrl = new Uri("http://172.17.198.145:5000/umbraco/api/");
     

        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(baseUrl, "setas/GetMushrooms");
                var items = Enumerable.Empty<Mushroom>();

                try
                {
                    var response = await client.GetAsync(uri);


                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        items = JsonConvert.DeserializeObject<List<Mushroom>>(content);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return items;
            }
        }


        public async Task<Configuration> GetConfigurationAsync()
        {
            Configuration configuration = null;
            using(var client = new HttpClient())
            {
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
