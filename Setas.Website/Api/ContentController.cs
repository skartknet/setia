using System;
using System.Linq;
using System.Web.Http;
using Setas.Common.Enums;
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
            var nodeExists = _mushroomsRoot.Children.FirstOrDefault(n => n.Name.Equals(content.Name, System.StringComparison.OrdinalIgnoreCase)) != null;

            IContent node;
            if (!nodeExists)
            {
                //new node
                node = Services.ContentService.CreateContentWithIdentity(content.Name, _mushroomsRoot.Id, Mushroom.ModelTypeAlias);

                node.SetValue("class", content.Class);
                node.SetValue("confusion", content.Confusion);
                node.SetValue("cookingInstructions", content.CookingInstructions);
                node.SetValue("cookingInterest",  content.CookingInterest.ToString());
                node.SetValue("description", content.Description);
                node.SetValue("family", content.Family);
                node.SetValue("habitat", content.Habitat);
                node.SetValue("order", content.Order);
                node.SetValue("popularNames", content.PopularNames);
                node.SetValue("season", content.Season);
                node.SetValue("subclass", content.Subclass);
                node.SetValue("synonyms", content.Synonyms);

                Services.ContentService.Save(node);
                return node.Id;

            }
            //else
            //{
            //    //node exists, we update info.
            //    node = Services.ContentService.GetById(existingNode.First().Id);
            //}

            return default(int);
        }

        [HttpGet]
        public int? GetNodeId(string name)
        {
            var node = Services.ContentService.GetDescendants(_mushroomsRoot.Id)
                .FirstOrDefault(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

            if (node == null) return null;
            return node.Id;

        }

    }
}