using Autofac;
using AxisAvaloniaApp.Services.Activation;

namespace AxisAvaloniaApp.AutofacModules
{
    public sealed class ActivationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActivationService>().As<IActivationService>().InstancePerLifetimeScope();
        }
    }
}
