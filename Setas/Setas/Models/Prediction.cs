using Xamarin.Forms;

namespace Setas.Models
{
    public class Prediction
    {
        //Prediction details
        public string TagId { get; set; }
        public string TagName { get; set; }
        public float Probability { get; set; }

        //Item details
        public string Name { get; set; }
        public string PopularName { get; set; }
        public ImageSource Image { get; set; }
    }
}
