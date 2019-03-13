using System.Linq;
using System.Web.Http;
using Setas.Core.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using ApiModels = Setas.Common.Models.Api;

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
        public int CreateNode(ApiModels.Mushroom content)
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

        [HttpGet]
        public bool NodeExists(string name)
        {
            var node = Services.ContentService.GetDescendants(_mushroomsRoot.Id)
                .FirstOrDefault(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

            return node != null;

        }

    }
}