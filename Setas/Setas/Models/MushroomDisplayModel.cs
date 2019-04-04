using System;
using System.Linq;
using Setas.Data;
using Setas.Enums;
using Setas.ViewModels.Helpers;

namespace Setas.Models
{
    public class MushroomDisplayModel : Mushroom
    {
        public MushroomDisplayModel(Mushroom m)
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
            ImageLicense = m.ImageLicense;
        }

        public string EdibleAsReadableString
        {
            get
            {
                return EdibleHelpers.EdibleEnumToReadableString(CookingInterest);
            }
        }

        public Uri ImageSource
        {
            get
            {
                var uri = new Uri(App.ExternalService, this.Image);
                return uri;
            }
        }

        public string FirstPopularName
        {
            get
            {
                if (!string.IsNullOrEmpty(PopularNames))
                {
                    return PopularNames.Split(',').First();
                }

                return string.Empty;
            }
        }


        public bool IsToxic
        {
            get
            {
                return EdibleHelpers.EdibleToTopClass(CookingInterest) == EdibleTopClass.Toxic;
            }
        }

        public bool IsNoInterest
        {
            get
            {
                return EdibleHelpers.EdibleToTopClass(CookingInterest) == EdibleTopClass.NoInterest;
            }
        }

        public bool IsSafe
        {
            get
            {
                return EdibleHelpers.EdibleToTopClass(CookingInterest) == EdibleTopClass.Safe;
            }
        }

        public bool IsWarning
        {
            get
            {
                return EdibleHelpers.EdibleToTopClass(CookingInterest) == EdibleTopClass.Warning;
            }
        }
    }
}
