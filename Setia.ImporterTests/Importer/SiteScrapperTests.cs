using System;
using CustomVisionImporter.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Setas.Xamarin.Tests.Importer
{
    [TestClass]
    public class SiteScrapperTests
    {
        [TestMethod]
        public void Scrapper()
        {
            var service = new SiteScrapperService();
            service.GetContent("Agaricus Campestris");
        }
    }
}
