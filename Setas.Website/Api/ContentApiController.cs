using AutoMapper;
using Setas.Core.Binding;
using Setas.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using ApiModels = Setas.Website.Api.Models;



namespace Setas.Website.Api
{
    public class ContentController : UmbracoApiController
    {
        public IEnumerable<ApiModels.Mushroom> GetMushrooms([ModelBinder(typeof(CommaDelimitedArrayModelBinder))] int[] contentIds)
        {

            var catalogue = Umbraco.TypedContentAtRoot().First(n => n.DocumentTypeAlias == Catalogue.ModelTypeAlias);

            var items = Enumerable.Empty<Mushroom>();

            if (contentIds != null && contentIds.Any())
            {
                items = catalogue.Children(n => contentIds.Contains(n.Id)).OfType<Mushroom>();
            }
            else
            {
                items = catalogue.Children().OfType<Mushroom>();
            }

            return Mapper.Map<IEnumerable<ApiModels.Mushroom>>(items);

        }


    }
}