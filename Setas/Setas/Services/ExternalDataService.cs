using Newtonsoft.Json;
using Setas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Setas.Services
{

    public class ExternalDataService : IDataService
    {

        readonly Uri baseUrl = new Uri("http://172.17.198.145:5000/umbraco/api/");
        private static readonly object padlock = new object();

        private static ExternalDataService _instance;

        public static ExternalDataService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ExternalDataService();
                    }
                    return _instance;
                }
            }
        }


        private ExternalDataService()
        {

        }
      


        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(params int[] ids)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(baseUrl, "content/GetMushrooms");
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


        public async Task<Mushroom> GetMushroomAsync(int id)
        {
            using(HttpClient client = new HttpClient())
            {
                var uri = new Uri(baseUrl, "content/GetMushroom");
                Mushroom item = null;

                try
                {
                    var response = await client.GetAsync(uri);


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
    }
}
