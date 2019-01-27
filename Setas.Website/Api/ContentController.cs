using Setas.Common.Models.Api;
using Setas.Core.Models;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace Setas.Website.Api
{
    public class ContentController : UmbracoApiController
    {

        IPublishedContent _mushroomsRoot;

        public ContentController()
        {
            _mushroomsRoot = Umbraco.TypedContentAtRoot().First(m => m.DocumentTypeAlias == Catalogue.ModelTypeAlias);
        }

        [HttpPost]
        public int CreateNode(ImportNodeContent content)
        {
            var existingNode = Umbraco.TypedSearch(content.Name);
            IContent node;
            if (!existingNode.Any())
            {
                //new node
                node = Services.ContentService.CreateContentWithIdentity(content.Name, _mushroomsRoot.Id, Mushroom.ModelTypeAlias);
            }
            else
            {
                //node exists, we update info.
                node = Services.ContentService.GetById(existingNode.First().Id);
            }

            return node.Id;
        }

    }
}