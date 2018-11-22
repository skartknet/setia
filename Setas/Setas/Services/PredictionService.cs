using Newtonsoft.Json;
using Setas.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Setas.Services
{


    public class PredictionService : IPredictionService
    {


        private const string predictionKey = "d6443721d97b46479be1634493aa83e2";
        private const string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/2e7aba89-bdde-479f-9b27-be098914db6a/image";

        public async Task<PredictionResponse> Analyse(byte[] byteData)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                HttpResponseMessage response;


                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(url, content);
                    var responseResult = await response.Content.ReadAsStringAsync();

                    var predictionResult = JsonConvert.DeserializeObject<PredictionResponse>(responseResult);

                    return predictionResult;
                }
            }
        }
    }
}
