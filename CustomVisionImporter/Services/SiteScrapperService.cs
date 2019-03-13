using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace CustomVisionImporter.Services
{
    public class SiteScrapperService
    {
        const string WebsiteBase = "http://www.fichasmicologicas.com";
        public void GetContent(string name)
        {
            var web1 = new HtmlWeb();
            var doc1 = web1.Load(WebsiteBase + "/index.php" + "?s=" + name);

            doc1.GetElementbyId("")
        }
    }
}
