using Autofac;
using AxisAvaloniaApp.Services.Settings;

namespace AxisAvaloniaApp.AutofacModules
{
    /// <summary>
    /// Register settings service.
    /// </summary>
    public sealed class SettingsModule : Module
    {
        /// <summary>
        /// Add registration to container.
        /// </summary>
        /// <param name="builder">Container in which service will be added.</param>
        /// <date>16.03.2022.</date>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SettingsService>().As<ISettingsService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
        }
    }
}
