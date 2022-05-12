using Autofac;
using AxisAvaloniaApp.Services.SearchNomenclatureData;

namespace AxisAvaloniaApp.AutofacModules
{
    /// <summary>
    /// Register settings service.
    /// </summary>
    public sealed class SearchDataModule : Module
    {
        /// <summary>
        /// Add registration to container.
        /// </summary>
        /// <param name="builder">Container in which service will be added.</param>
        /// <date>17.03.2022.</date>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearchDataService>().As<ISearchData>().InstancePerLifetimeScope();
            builder.RegisterType<SearchDataService>().As<ISearchData>().SingleInstance();
        }
    }
}
