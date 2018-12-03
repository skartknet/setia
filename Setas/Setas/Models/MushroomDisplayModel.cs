using Setas.Common.Models;
using System;

namespace Setas.Models
{
    public class MushroomDisplayModel : MushroomData
    {
        public MushroomDisplayModel(MushroomData m)
        {
            Class = m.Class;
            Confusion = m.Confusion;
            CookingInstructions = m.CookingInstructions;
            CookingInterest = m.CookingInterest;
            Description = m.Description;
            Family = m.Family;
            Habitat = m.Habitat;
            Id = m.Id;
            Image = m.Image;
            Name = m.Name;
            Order = m.Order;
            PopularNames = m.PopularNames;
            Season = m.Season;
            Subclass = m.Subclass;
            Synonyms = m.Synonyms;
        }
        public Xamarin.Forms.ImageSource ImageSource
        {
            get
            {
                
                var image = Xamarin.Forms.ImageSource.FromUri(new Uri(App.ExternalService, this.Image));
                return image;
            }
        }
    }
}
