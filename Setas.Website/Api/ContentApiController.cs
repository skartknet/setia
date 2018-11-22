using AutoMapper;
using Setas.Core.Binding;
using Setas.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.ModelBinding;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using ApiModels = Setas.Website.Api.Models;



namespace Setas.Website.Api
{
    public class ContentController : UmbracoApiController
    {
        public HttpResponseMessage GetMushrooms([ModelBinder(typeof(CommaDelimitedArrayModelBinder))] int[] ids)
        {

            var catalogue = Umbraco.TypedContentAtRoot().First(n => n.DocumentTypeAlias == Catalogue.ModelTypeAlias);

            var items = Enumerable.Empty<Mushroom>();

            if (ids != null && ids.Any())
            {
                items = catalogue.Children(n => ids.Contains(n.Id)).OfType<Mushroom>();
            }
            else
            {
                items = catalogue.Children().OfType<Mushroom>();
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
            var item  = catalogue.FirstChild<Mushroom>(n => n.Id == id);

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


    }
}