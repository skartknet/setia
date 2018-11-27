using AutoMapper;
using Setas.Common.Enums;
using Setas.Common.Models;
using Setas.Core.Binding;
using Setas.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.ModelBinding;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using ApiModels = Setas.Common.Models;
using CoreModels = Setas.Core.Models;


namespace Setas.Website.Api
{
    public class SetasController : UmbracoApiController
    {
        public HttpResponseMessage GetMushrooms([ModelBinder(typeof(CommaDelimitedArrayModelBinder))] int[] ids, Edible? edible = null)
        {

            var catalogue = Umbraco.TypedContentAtRoot().First(n => n.DocumentTypeAlias == Catalogue.ModelTypeAlias);

            var items = Enumerable.Empty<CoreModels.Mushroom>();

            if (ids != null && ids.Any())
            {
                items = catalogue.Children(n => ids.Contains(n.Id)).OfType<CoreModels.Mushroom>();
            }
            else
            {
                items = catalogue.Children().OfType<CoreModels.Mushroom>();
            }

            if(edible != null)
            {
                items = items.Where(m => ((Edible)m.CookingInterest.SavedValue) == edible);
            }


            IEnumerable<ApiModels.Mushroom> itemsMapped = null;

            try
            {
                itemsMapped = Mapper.Map<IEnumerable<ApiModels.Mushroom>>(items);
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error mapping models");
            }

            return Request.CreateResponse(HttpStatusCode.OK, itemsMapped);

        }

        public HttpResponseMessage GetMushroom(int id)
        {
            var catalogue = Umbraco.TypedContentAtRoot().First(n => n.DocumentTypeAlias == Catalogue.ModelTypeAlias);
            var item  = catalogue.FirstChild<CoreModels.Mushroom>(n => n.Id == id);

            if (item == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found.");

            ApiModels.Mushroom itemMapped = null;
            try
            {
                itemMapped = Mapper.Map<ApiModels.Mushroom>(item);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error mapping model");
            }

            return Request.CreateResponse(HttpStatusCode.OK, itemMapped);
            
        }

        public HttpResponseMessage GetConfiguration()
        {
            var database = DatabaseContext.Database;

            var config = database.FirstOrDefault<Configuration>(new Sql().Select("*").From("SetasConfiguration"));

            return Request.CreateResponse(config);
        }
    }
}