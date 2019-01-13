using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomVisionImporter.Services
{
    class CustomVisionService
    {
        private HttpClient _client => new HttpClient();

        public Uri BaseUrl { get; }
        public string ProjectId { get; }


        public CustomVisionService(Uri baseUrl, string projectId)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new ArgumentException("A project id is needed", nameof(projectId));
            }




        }

        async Task CreateImagesFromFiles()
        {

            var endpoint = "/images/files";


            await _client.PostAsync(new Uri(BaseUrl, endpoint));

        }



    }
}
