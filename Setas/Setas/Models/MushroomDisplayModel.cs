using Setas.Data;
using Setas.Enums;
using System;

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
        }
        public Uri ImageSource
        {
            get
            {
                var uri = new Uri(App.ExternalService, this.Image);
                return uri;
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


        public string EdibleAsReadableString
        {
            get
            {

                switch (CookingInterest)
                {
                    case Common.Enums.Edible.BuenComestible:
                        return "Comestible";
                    case Common.Enums.Edible.ComestibleBajaCalidad:
                        return "Comestible Baja Calidad";
                    case Common.Enums.Edible.ComestibleCalidadMedia:
                        return "Comestible Calidad Media";
                    case Common.Enums.Edible.Toxica:
                        return "Tóxica";
                    case Common.Enums.Edible.SinInteres:
                        return "Sin Interés";
                    case Common.Enums.Edible.PosibleToxico:
                        return "Posible Tóxica";
                    case Common.Enums.Edible.ComestibleConPrecaucion:
                        return "Comestible con Precaución";
                    case Common.Enums.Edible.ComestiblePeroPeligrosa:
                        return "Comestible pero peligrosa";
                    case Common.Enums.Edible.NoComestible:
                        return "No comestible";
                    default:
                        return "Desconocido";
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
