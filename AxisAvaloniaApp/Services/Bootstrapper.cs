using Autofac;
using System;
using AxisAvaloniaApp.AutofacModules;
using Autofac.Extensions.DependencyInjection;
using Splat;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.ThemeSelector;
using AxisAvaloniaApp.ViewModels;
using DataBase.Repositories.ApplicationLog;
using DataBase;
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
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.Services.Scanning;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using AxisAvaloniaApp.Services.Document;
using AxisAvaloniaApp.Services.AxisCloud;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Services.Validation;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Navigation;
using Avalonia.Platform;
using AxisAvaloniaApp.Services.Printing;
using Avalonia;

namespace AxisAvaloniaApp.Services
{
    /// <summary>
    /// Class to register services.
    /// </summary>
    internal static class Bootstrapper
    {
        [Obsolete]
        public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register<IActivationService>(() => new ActivationService(
                resolver.GetRequiredService<ISettingsService>(), resolver.GetRequiredService<IThemeSelectorService>()));

            services.Register<INavigationService>(() => new NavigationService());
            services.RegisterLazySingleton<IThemeSelectorService>(() => new ThemeSelectorService());
            services.RegisterLazySingleton<ISettingsService>(() => new SettingsService(
                resolver.GetRequiredService<ISettingsRepository>(),
                resolver.GetRequiredService<IOperationHeaderRepository>(),
                resolver.GetRequiredService<IPaymentService>()));
            services.Register<ISerializationService>(() => new SerializationService(resolver.GetRequiredService<ISerializationRepository>()));
            services.RegisterLazySingleton<IScanningData>(() => new ScanningService(resolver.GetRequiredService<ISettingsService>()));
            services.RegisterLazySingleton<IPaymentService>(() => new PaymentService());
            services.RegisterLazySingleton<ISearchData>(() => new SearchDataService());
            services.Register<IDocumentService>(() => new DocumentService(resolver.GetRequiredService<ISettingsService>()));
            services.RegisterLazySingleton<IAxisCloudService>(() => new AxisCloudService());
            services.RegisterLazySingleton<ITranslationService>(() => new TranslationService(resolver.GetRequiredService<ISettingsService>()));
            services.RegisterLazySingleton<ILoggerService>(() => new LoggerService());
            services.RegisterLazySingleton<IValidationService>(() => new ValidationService());

            switch (AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem)
            {
                case OperatingSystemType.WinNT:
                    services.Register<IPrintService>(() => new WindowsPrintService());
                    break;
                case OperatingSystemType.Linux:
                    services.Register<IPrintService>(() => new LinuxPrintService());
                    break;
                case OperatingSystemType.OSX:
                    services.Register<IPrintService>(() => new MacOSPrintService());
                    break;
                default:
                    throw new NotImplementedException("Service to print doesn't implemented!");
            }

            RegisterViewModels(services);
            RegisterRepositories(services, resolver);
        }

        private static void RegisterViewModels(IMutableDependencyResolver services)
        {
            services.Register(() => new MainWindowViewModel());
            services.Register(() => new SaleViewModel());
            services.Register(() => new InvoiceViewModel());
            services.Register(() => new ProformInvoiceViewModel());
            services.Register(() => new DebitNoteViewModel());
            services.Register(() => new CreditNoteViewModel());
            services.Register(() => new CashRegisterViewModel());
            services.Register(() => new ExchangeViewModel());
            services.Register(() => new ReportsViewModel());
            services.Register(() => new SettingsViewModel());
        }

        private static void RegisterRepositories(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.RegisterLazySingleton(() => new DatabaseContext(Configurations.DatabaseConfiguration.GetOptions()));
            services.Register<IApplicationLogRepository>(() => new ApplicationLogRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IDocumentsRepository>(() => new DocumentsRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IExchangesRepository>(() => new ExchangesRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IItemRepository>(() => new ItemRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IItemsCodesRepository>(() => new ItemsCodesRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IItemsGroupsRepository>(() => new ItemsGroupsRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IOperationDetailsRepository>(() => new OperationDetailsRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IOperationHeaderRepository>(() => new OperationHeaderRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IPartnerRepository>(() => new PartnerRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IPartnersGroupsRepository>(() => new PartnersGroupsRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IPaymentTypesRepository>(() => new PaymentTypesRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<ISerializationRepository>(() => new SerializationRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<ISettingsRepository>(() => new SettingsRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));
            services.Register<IVATsRepository>(() => new VATsRepository((DatabaseContext)resolver.GetRequiredService(typeof(DatabaseContext))));

        }

        /// <summary>
        /// Configures services.
        /// </summary>
        /// <returns>AutofacServiceProvider.</returns>
        internal static IServiceProvider ConfigureServices()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterModule<ActivationModule>();
            //builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<MediatorModule>();
            builder.RegisterModule<ProcessingModule>();
            // builder.RegisterModule<ApplicationServicesModule>();
            //builder.RegisterModule(
            //    new DatabaseModule(
            //        Configurations.DatabaseConfiguration.GetOptions()));
            //builder.RegisterModule<SettingsModule>();
            //builder.RegisterModule<SerializationModule>();
            //builder.RegisterModule<ScanningModule>();
            //builder.RegisterModule<PaymentModule>();
            //builder.RegisterModule<SearchDataModule>();
            //builder.RegisterModule<DocumentModule>();
            //builder.RegisterModule<AxisCloudModule>();

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
