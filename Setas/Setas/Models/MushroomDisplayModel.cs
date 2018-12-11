using Setas.Common.Models;
using Setas.Enums;
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
        public EdibleTopClassEnum EdibleOverview
        {
            get
            {
                switch (CookingInterest)
                {
                    case Common.Enums.Edible.BuenComestible:
                    case Common.Enums.Edible.ComestibleBajaCalidad:
                    case Common.Enums.Edible.ComestibleCalidadMedia:
                        return EdibleTopClassEnum.Safe;
                    case Common.Enums.Edible.Toxica:
                        return EdibleTopClassEnum.Toxic;
                    case Common.Enums.Edible.SinInteres:
                        return EdibleTopClassEnum.NoInterest;
                    case Common.Enums.Edible.PosibleToxico:
                    case Common.Enums.Edible.ComestibleConPrecaucion:
                    case Common.Enums.Edible.ComestiblePeroPeligrosa:
                    case Common.Enums.Edible.NoComestible:
                        return EdibleTopClassEnum.Warning;
                    default:
                        return EdibleTopClassEnum.Warning;
                }
            }
        }

        public bool IsToxic
        {
            get
            {
                return EdibleOverview == EdibleTopClassEnum.Toxic;
            }
        }

        public bool IsNoInterest
        {
            get
            {
                return EdibleOverview == EdibleTopClassEnum.NoInterest;
            }
        }

        public bool IsSafe
        {
            get
            {
                return EdibleOverview == EdibleTopClassEnum.Safe;
            }
        }

        public bool IsWarning
        {
            get
            {
                return EdibleOverview == EdibleTopClassEnum.Warning;
            }
        }
    }
}
