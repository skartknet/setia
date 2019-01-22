using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CustomVisionImporter.Services
{
    class CustomVisionService
    {
        private HttpClient _client => new HttpClient();

        public Uri BaseUrl { get; }
        public string ProjectId { get; }
        public string TrainingKey { get; }


        public CustomVisionService(Uri baseUrl, string projectId, string trainingKey)
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

        public async Task CreateImagesFromFilesAsync(IEnumerable<string> files)
        {

            var endpoint = $"projects/{ProjectId}/images/files";

            _client.DefaultRequestHeaders.Add("Training-Key", "");
            _client.DefaultRequestHeaders.Add("Training-Key", TrainingKey);

            // Request body

            byte[] byteData = await EncodeFiles(files);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await _client.PostAsync(new Uri(BaseUrl, endpoint), content);
            }


        }

        private async Task<byte[]> EncodeFiles(IEnumerable<string> files)
        {
            using (var ms = new MemoryStream())
            {
                foreach (var item in files)
                {
                    yield return await File.ReadAllBytesAsync(item);
                }
            }
        }
    }
}
