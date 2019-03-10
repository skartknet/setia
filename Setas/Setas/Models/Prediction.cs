
using System.Globalization;

namespace Setas.Models
{
    public class Prediction
    {
        //Prediction details
        public double Probability { get; set; }

        public string ProbabilityAsPercentage
        {
            get
            {
                return Probability.ToString("P0", CultureInfo.InvariantCulture);
            }
        }

        public MushroomDisplayModel Mushroom { get; set; }
    }
}
