using AutoMapper;
using Setas.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;
using ApiModels = Setas.Website.Api.Models;
using Umbraco.Web;


namespace Setas.Website.Api
{
    class SetiaMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<Mushroom, ApiModels.Mushroom>()
                .ForMember(n=>n.Images, exp=>exp.ResolveUsing(p=>((Mushroom) p).Images.Select(img=>img.Url)));
        }
    }
}