using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Setas.Common.Enums;

namespace CustomVisionImporter.Services
{
    public class SiteScrapperService
    {
        const string WebsiteBase = "http://www.fichasmicologicas.com";
        public static async Task<IDocument> GetContentPage(string name)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var browser = BrowsingContext.New(config);
            IDocument resultsPage;
            IDocument nodePage;

            try
            {
                resultsPage = await browser.OpenAsync(WebsiteBase + "/index.php" + "?s=" + name);
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't retrieve results page", ex);
            }

            var anchor = resultsPage.QuerySelector("#articulos_container")?.Descendents().OfType<IHtmlAnchorElement>().FirstOrDefault(node => node.Text.Trim().Equals(name, StringComparison.OrdinalIgnoreCase));

            if (anchor == null)
            {
                return null;
            }

            var pageUrl = ((Element)anchor).GetAttribute("href");

            if (pageUrl == null)
            {
                return null;
            }

            try
            {
                nodePage = await browser.OpenAsync(new Url(WebsiteBase + pageUrl));

            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't retrieve details page", ex);
            }

            return nodePage;
        }
          
        
        public static Edible EdibleStringToEnum(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Edible string cannot be empty", nameof(name));
            }

            Edible result = default;
            switch (name)
            {
                case "Buen comestible":
                    result = Edible.BuenComestible;
                    break;
                case "Tóxica":
                    result = Edible.Toxica;
                    break;
                case "Sin interés culinario":
                    result = Edible.SinInteres;
                    break;
                case "Comestible con precauciones":
                    result = Edible.ComestibleConPrecaucion;
                    break;
                case "Comestible pero peligrosa":
                    result = Edible.ComestiblePeroPeligrosa;
                    break;
                case "Comestible de calidad media":
                    result = Edible.ComestibleCalidadMedia;
                    break;
                case "Comestible de baja calidad":
                    result = Edible.ComestibleBajaCalidad;
                    break;
                case "Comestible dudoso, posible toxicidad":
                    result = Edible.PosibleToxico;
                    break;
                case "No comestible generalmente":
                    result = Edible.NoComestible;
                    break;              
            }

            return result;
        }
    }
}
