using AutoMapper;
using ApiModels = Setas.Common.Models.Api;
using DataModels = Setas.Data;

namespace Setas.Models.Mapping
{
    public class MappingConfiguration
    {
        public static void Init()
        {


            Mapper.Initialize(cfg =>
                cfg.CreateMap<ApiModels.Mushroom, DataModels.Mushroom>()
                );
        }

    }
}
