using Autofac;
using Setas.Enums;

namespace Setas.Services
{
    class DependencyContainer
    {
        public static IContainer Container { get; private set; }

        public static void Register(App app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ExternalDataService>().As<IExternalDataService>().SingleInstance();
            builder.RegisterType<InternalDataService>().As<IInternalDataService>().SingleInstance();

            builder.RegisterType<SyncingDataService>().As<ISyncingDataService>().SingleInstance();



            builder.RegisterType<PredictionService>().As<IPredictionService>().SingleInstance();

            Container = builder.Build();
        }
    }
}
