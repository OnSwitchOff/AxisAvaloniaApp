using Autofac;
using DataBase;
using DataBase.Repositories.ApplicationLog;
using DataBase.Repositories.Documents;
using DataBase.Repositories.Exchanges;
using DataBase.Repositories.Items;
using DataBase.Repositories.ItemsCodes;
using DataBase.Repositories.ItemsGroups;
using DataBase.Repositories.OperationDetails;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Partners;
using DataBase.Repositories.PartnersGroups;
using DataBase.Repositories.PaymentTypes;
using DataBase.Repositories.Serializations;
using DataBase.Repositories.Settings;
using DataBase.Repositories.VATGroups;
using Microsoft.EntityFrameworkCore;

namespace AxisAvaloniaApp.AutofacModules
{
    /// <summary>
    /// Register database service.
    /// </summary>
    public sealed class DatabaseModule : Module
    {
        private readonly DbContextOptions<DatabaseContext> dbContextOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseModule"/> class.
        /// </summary>
        /// <param name="dbContextOptions">Class to communicate with database.</param>
        public DatabaseModule(DbContextOptions<DatabaseContext> dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
        }

        /// <summary>
        /// Add registration to container.
        /// </summary>
        /// <param name="builder">Container in which service will be added.</param>
        /// <date>16.02.2022.</date>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>().WithParameter("options", this.dbContextOptions).SingleInstance();

            builder.RegisterType<ApplicationLogRepository>().As<IApplicationLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentsRepository>().As<IDocumentsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ExchangesRepository>().As<IExchangesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ItemRepository>().As<IItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ItemsCodesRepository>().As<IItemsCodesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ItemsGroupsRepository>().As<IItemsGroupsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OperationDetailsRepository>().As<IOperationDetailsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OperationHeaderRepository>().As<IOperationHeaderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PartnerRepository>().As<IPartnerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PartnersGroupsRepository>().As<IPartnersGroupsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentTypesRepository>().As<IPaymentTypesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SerializationRepository>().As<ISerializationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VATsRepository>().As<IVATsRepository>().InstancePerLifetimeScope();
        }
    }
}
