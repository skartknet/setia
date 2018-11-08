using System;

namespace Setas.Models
{
 
    class PredictionResponse
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public DateTime Created { get; set; }
        public Prediction[] Predictions { get; set; }
    }



}
