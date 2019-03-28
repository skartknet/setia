using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Setas.Helpers
{
    public static class PredictionsExtensions
    {
        /// <summary>
        /// Extracts the id form a Tag of type 123:name
        /// </summary>
        public static int CmsNodeId(this PredictionModel model)
        {
            int id = default(int);
            try
            {
                id = int.Parse(model.TagName.Split(':')[0]);
            }
            catch
            { }

            return id;
        }
    }
}
