
using System.Globalization;

namespace Setas.Models
{
    public class Prediction
    {
        //Prediction details
        public string TagId { get; set; }
        public string TagName { get; set; }
        public float Probability { get; set; }

        public string ProbabilityAsPercentage
        {
            get
            {
                return Probability.ToString("P0", CultureInfo.InvariantCulture);
            }
        }

        public Mushroom Mushroom { get; set; }
    }
}
