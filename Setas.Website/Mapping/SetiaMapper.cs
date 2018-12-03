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

            config.CreateMap<Mushroom, ApiModels.MushroomData>()
                .ForMember(n => n.Image, exp => exp.ResolveUsing(p => p.Images != null && p.Images.Any() ?
                                                                     string.Join(",", p.Images.Select(img => img.Url)) :
                                                                     null))
                .ForMember(n => n.CookingInstructions, exp => exp.ResolveUsing<HtmlResolver>().FromMember(n => n.CookingInstructions))

                .ForMember(n => n.Confusion, exp => exp.ResolveUsing<HtmlResolver>().FromMember(n => n.Confusion))

                .ForMember(n => n.Description, exp => exp.ResolveUsing<HtmlResolver>().FromMember(n => n.Description))

                .ForMember(n => n.Habitat, exp => exp.ResolveUsing<HtmlResolver>().FromMember(n => n.Habitat))

                .ForMember(n => n.Season, exp => exp.ResolveUsing<HtmlResolver>().FromMember(n => n.Season))

                //.ForMember(n => n.PopularNames, exp => exp.ResolveUsing<StringListResolver>(p => ((Mushroom)p).PopularNames.Select(s => s)))

                .ForMember(n => n.CookingInterest, exp => exp.ResolveUsing(p => (Edible)Enum.Parse(typeof(Edible), ((Mushroom)p).CookingInterest.SavedValue.ToString())));
        }
    }
}