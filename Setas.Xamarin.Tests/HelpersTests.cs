using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Setas.Helpers;

namespace Setas.Xamarin.Tests
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void TagToItemId()
        {
            var id = Predictions.TagToItemId("123:asd");

            Assert.AreEqual(123, id);
        }
    }
}
