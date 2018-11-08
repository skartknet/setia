namespace Setas.Models
{
    public class Prediction
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public float Probability { get; set; }



        //UI properties
        public string Title { get => $"{TagName} - {Probability}"; }
    }
}
