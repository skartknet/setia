using Setas.Common.Enums;
using Setas.Enums;

namespace Setas.ViewModels.Helpers
{
    public static class EdibleHelpers
    {
        public static string EdibleEnumToReadableString(Edible value)
        {
            switch (value)
            {
                case Edible.BuenComestible:
                    return "Comestible";
                case Edible.Toxica:
                    return "Tóxica";
                case Edible.SinInteres:
                    return "Sin interés";
                case Edible.ComestibleConPrecaucion:
                    return "Comestible con precaución";
                case Edible.ComestiblePeroPeligrosa:
                    return "Comestible pero peligrosa";
                case Edible.ComestibleCalidadMedia:
                    return "Comestible de calidad media";
                case Edible.ComestibleBajaCalidad:
                    return "Comestible de baja calidad";
                case Edible.PosibleToxico:
                    return "Posible tóxico";
                case Edible.NoComestible:
                    return "No comestible";
                default:
                    return string.Empty;
            }
        }

        public static string EdibleTopClassToReadableString(EdibleTopClass value)
        {
            switch (value)
            {
                case EdibleTopClass.Safe:
                    return "Seguro";
                case EdibleTopClass.Toxic:
                    return "Tóxico";
                case EdibleTopClass.NoInterest:
                    return "Sin interés";
                case EdibleTopClass.Warning:
                    return "Precaución";
                default:
                    return string.Empty;
            }
        }

        public static EdibleTopClass EdibleToTopClass(Edible value)
        {
            switch (value)
            {
                case Common.Enums.Edible.BuenComestible:
                case Common.Enums.Edible.ComestibleBajaCalidad:
                case Common.Enums.Edible.ComestibleCalidadMedia:
                    return EdibleTopClass.Safe;
                case Common.Enums.Edible.Toxica:
                    return EdibleTopClass.Toxic;
                case Common.Enums.Edible.SinInteres:
                    return EdibleTopClass.NoInterest;
                case Common.Enums.Edible.PosibleToxico:
                case Common.Enums.Edible.ComestibleConPrecaucion:
                case Common.Enums.Edible.ComestiblePeroPeligrosa:
                case Common.Enums.Edible.NoComestible:
                    return EdibleTopClass.Warning;
                default:
                    return EdibleTopClass.Warning;
            }
        }
    }
}
