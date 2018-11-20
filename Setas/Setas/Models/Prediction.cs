
namespace Setas.Models
{
    public class Prediction
    {
        //Prediction details
        public string TagId { get; set; }
        public string Tag { get; set; }
        public float Probability { get; set; }        

        public Mushroom Mushroom { get; set; }
    }
}
