using System;
using System.Collections.Generic;
using System.Text;
using Umbraco.Headless.Client.Net.Services;

namespace Setas.Services.Headless
{
    public class HeadlessClient
    {
        public static PublishedContentService Instance { get; } = new PublishedContentService(App.HeadlessUrl);
    }
}
