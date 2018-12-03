using AutoMapper;
using System.Web;
using Umbraco.Web;

namespace Setas.Website.Api
{
    public class HtmlResolver : ValueResolver<IHtmlString, string>
    {
        protected override string ResolveCore(IHtmlString source)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);            

            source = helper.StripHtml(source);

            return source.ToString();
        }
    }
}