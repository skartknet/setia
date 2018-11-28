using AutoMapper;
using Setas.Common.Enums;
using Setas.Core.Models;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;
using Umbraco.Web;
using ApiModels = Setas.Common.Models;


namespace Setas.Website.Api
{
    class SetiaMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<Mushroom, ApiModels.Mushroom>()
                .ForMember(n => n.Images, exp => exp.ResolveUsing(p => p.Images != null && p.Images.Any() ?
                                                                     string.Join(",", p.Images.Select(img => img.Url)) :
                                                                     null))
                .ForMember(n => n.PopularNames, exp => exp.ResolveUsing(p => ((Mushroom)p).PopularNames.ToArray()))
                .ForMember(n => n.CookingInterest, exp => exp.ResolveUsing(p => (Edible)Enum.Parse(typeof(Edible), ((Mushroom)p).CookingInterest.SavedValue.ToString())));
        }
    }
}