using System;
using System.Linq;
using AngleSharp.Dom;

namespace CustomVisionImporter.Services.Extensions
{
    public static class ScrapperExtensions
    {
        /// <summary>
        /// It returns de value for a field in the Mushrrom details page.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="labelText">The text for the label preceding the text node</param>
        /// <returns></returns>
        public static string GetMushroomDetailsValue(this IDocument document, string labelText)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var fieldText = document.QuerySelectorAll(".m_label")
                   .FirstOrDefault(c => c.TextContent.Equals(labelText))?
                   .NextElementSibling?
                   .Text()?
                   .Trim();

            return fieldText;
        }

    }
}
