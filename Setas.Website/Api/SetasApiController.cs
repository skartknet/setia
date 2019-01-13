using AutoMapper;
using Setas.Common.Models;
using Setas.Core.Models;
using Setas.Website.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using ApiModels = Setas.Common.Models;
using CoreModels = Setas.Core.Models;


namespace Setas.Website.Api
{
    public class SetasController : UmbracoApiController
    {
        public HttpResponseMessage GetMushrooms(DateTime modifiedSince)
        {

            var catalogue = Umbraco.TypedContentAtRoot().First(n => n.DocumentTypeAlias == Catalogue.ModelTypeAlias);

            var items = Enumerable.Empty<IPublishedContent>();

            items = catalogue.Children().Where(i => i.UpdateDate >= modifiedSince);


            IEnumerable<ApiModels.MushroomData> itemsMapped = null;
            var mush = items.OfType<Mushroom>();
            try
            {
                itemsMapped = Mapper.Map<IEnumerable<ApiModels.MushroomData>>(mush);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error mapping models");
            }

            return Request.CreateResponse(HttpStatusCode.OK, itemsMapped);

        }

        public HttpResponseMessage GetMushroom(int id)
        {
            var catalogue = Umbraco.TypedContentAtRoot().First(n => n.DocumentTypeAlias == Catalogue.ModelTypeAlias);
            var item = catalogue.FirstChild<CoreModels.Mushroom>(n => n.Id == id);

            if (item == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found.");

            ApiModels.MushroomData itemMapped = null;
            try
            {
                itemMapped = Mapper.Map<ApiModels.MushroomData>(item);
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

            var configData = database.Fetch<ConfigurationData>(new Sql().Select("*").From(Setas.Website.Core.Constants.ConfigurationTableName));

            DateTime.TryParse(configData.FirstOrDefault(n => n.Alias == Setas.Common.Constants.LatestContentUpdatePropertyAlias)?.Value, out DateTime lastestUpdate);

            var configModel = new Configuration
            {
                LatestContentUpdate = lastestUpdate
            };

            return Request.CreateResponse(configModel);
        }
    }
}