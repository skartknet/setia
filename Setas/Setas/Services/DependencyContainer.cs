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

            switch (app.dataServiceType)
            {
                case SettingsDataType.Internal:
                    builder.RegisterType<InternalDataService>().As<IDataService>().SingleInstance();
                    break;
                case SettingsDataType.External:
                    builder.RegisterType<ExternalDataService>().As<IDataService>().SingleInstance();
                    break;
                default:
                    builder.RegisterType<ExternalDataService>().As<IDataService>().SingleInstance();
                    break;
            }

            Container = builder.Build();
        }
    }
}
