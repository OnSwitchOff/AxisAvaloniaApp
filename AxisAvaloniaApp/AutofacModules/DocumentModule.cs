using Autofac;
using AxisAvaloniaApp.Services.Document;

namespace AxisAvaloniaApp.AutofacModules
{
    /// <summary>
    /// Register service to generate pdf document.
    /// </summary>
    public sealed class DocumentModule : Module
    {
        /// <summary>
        /// Add registration to container.
        /// </summary>
        /// <param name="builder">Container in which service will be added.</param>
        /// <date>18.03.2022.</date>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentService>().As<IDocumentService>().InstancePerLifetimeScope();
        }
    }
}
