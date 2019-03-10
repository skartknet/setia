using Autofac;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Setas.Enums;



namespace Setas.Services
{
    class DependencyContainer
    {
        public static IContainer Container { get; private set; }

        public static void Register(App app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UmbracoExternalDataService>().As<IExternalDataService>().SingleInstance();
            builder.RegisterType<InternalDataService>().As<IInternalDataService>().SingleInstance();
            builder.RegisterType<SyncingDataService>().As<ISyncingDataService>().SingleInstance();
            builder.RegisterType<CustomVisionPredictionClient>().As<ICustomVisionPredictionClient>().SingleInstance();

            Container = builder.Build();
        }
    }
}
