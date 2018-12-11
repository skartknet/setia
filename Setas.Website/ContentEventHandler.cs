using Setas.Website.Core.Models.Data;
using System;
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
            var config = database.FirstOrDefault<ConfigurationData>(new Sql().Select("*").From(Setas.Website.Core.Constants.ConfigurationTableName).Where($"alias = '{Common.Constants.ContentUpdatedPropertyAlias}'"));
            if (config == null)
            {
                database.Insert(new ConfigurationData
                {
                    Alias = Common.Constants.ContentUpdatedPropertyAlias,
                    Value = DateTime.UtcNow.ToString()
                });
            }
            else
            {
                config.Value = DateTime.UtcNow.ToString();
                database.Update(config);
            }
        }
    }
}