using Newtonsoft.Json;
using Setas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Setas.Services
{

    public class ExternalDataService : IDataService
    {

        readonly Uri baseUrl = new Uri("http://172.17.198.145:5000/umbraco/api/");
     

        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(params int[] ids)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(baseUrl, "content/GetMushrooms");
                var items = Enumerable.Empty<Mushroom>();

                var query = HttpUtility.ParseQueryString(string.Empty);
                query["ids"] = string.Join(",", ids);


                try
                {
                    var response = await client.GetAsync(uri + "?" + query.ToString());


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


        public async Task<Mushroom> GetMushroomAsync(int id)
        {
            using(HttpClient client = new HttpClient())
            {
                var uri = new Uri(baseUrl, "content/GetMushroom");
                Mushroom item = null;
      

                try
                {
                    var response = await client.GetAsync(uri + "?id=" + id);


                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        item = JsonConvert.DeserializeObject<Mushroom>(content);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return item;
            }
        }

        public Task InsertMushroomsAsync(IEnumerable<Mushroom> sourceItems)
        {
            throw new NotImplementedException();
        }
    }
}
