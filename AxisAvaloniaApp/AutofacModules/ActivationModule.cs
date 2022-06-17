using Autofac;
using AxisAvaloniaApp.Services.StartUp;

namespace AxisAvaloniaApp.AutofacModules
{
    public sealed class ActivationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartUpService>().As<IStartUpService>().InstancePerLifetimeScope();
        }
    }
}
