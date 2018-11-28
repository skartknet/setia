using Setas.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Setas.Website
{
    public class ContentEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            ContentService.Published += ContentService_Published;
            ContentService.UnPublished += ContentService_Published;

        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            //TODO: move this to a service
            var database = ApplicationContext.Current.DatabaseContext.Database;
            var config = database.FirstOrDefault<Configuration>(new Sql().Select("*").From("SetasConfiguration"));
            if (config == null)
            {
                database.Insert("SetasConfiguration", "Id", new Configuration
                {
                    LatestContentUpdate = DateTime.UtcNow
                });
            }
            else
            {
                config.LatestContentUpdate = DateTime.UtcNow;
                database.Update("SetasConfiguration", "Id", config);
            }
        }
    }
}