using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Setas.Common.Models.Api;

namespace CustomVisionImporter.Services
{
    class UmbracoService
    {
        private HttpClient _client;

        public Uri ApiBase { get; set; }

        public UmbracoService()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Creates a node in Umbraco and returns the id of the new node. It returns 0 if unsuccessful.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<int> CreateNodeAsync(Mushroom content)
        {
            string endpoint = "content/createnode";

            var queryContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync(new Uri(ApiBase, endpoint), queryContent);

            if (result.IsSuccessStatusCode)
            {
                var rContent = await result.Content.ReadAsStringAsync();
                if (int.TryParse(rContent, out int nodeId))
                {
                    return nodeId;
                }
                else
                {
                    Console.WriteLine("Error parsing Umbraco node id: " + rContent);
                    return default(int);
                }
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        /// <summary>
        /// Check if a node already exists using the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<int?> GetMushroomIdAsync(string name)
        {
            string endpoint = "content/GetNodeId";
            var builder = new UriBuilder(new Uri(ApiBase, endpoint));
            builder.Query = $"name={name}";

            var result = await _client.GetAsync(builder.Uri);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                if (int.TryParse(content, out int id))
                {
                    return id;
                }
                return null;
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }
    }
}
