using System;
using Splat;
using AxisAvaloniaApp.Services.StartUp;
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
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Reports;
using AxisAvaloniaApp.Services.Reports.Bulgaria;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Services.Zip;
using AxisAvaloniaApp.Services.Crypto;
using AxisAvaloniaApp.Services.Exchange;

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
            services.Register<IStartUpService>(() => new StartUpService(
                resolver.GetRequiredService<ISettingsService>(),
                resolver.GetRequiredService<IScanningData>(),
                resolver.GetRequiredService<IPaymentService>(),
                resolver.GetRequiredService<IAxisCloudService>(),
                resolver.GetRequiredService<ILoggerService>(),
                resolver.GetRequiredService<ISearchData>(),
                resolver.GetRequiredService<IOperationHeaderRepository>()));

            services.Register<ICryptoService>(() => new CryptoService());
            services.Register<IActivationService>(() => new ActivationService("https://axis.sx/"));
            services.Register<INavigationService>(() => new NavigationService());
            services.RegisterLazySingleton<IThemeSelectorService>(() => new ThemeSelectorService());
            services.RegisterLazySingleton<ISettingsService>(() => new SettingsService(
                resolver.GetRequiredService<ISettingsRepository>(),
                resolver.GetRequiredService<ITranslationService>(),
                resolver.GetRequiredService<ISearchData>()));
            services.Register<ISerializationService>(() => new SerializationService(resolver.GetRequiredService<ISerializationRepository>()));
            services.RegisterLazySingleton<IScanningData>(() => new ScanningService(resolver.GetRequiredService<ISettingsService>()));
            services.RegisterLazySingleton<IPaymentService>(() => new PaymentService());
            services.RegisterLazySingleton<ISearchData>(() => new SearchDataService());
            services.Register<IDocumentService>(() => new DocumentService(resolver.GetRequiredService<ISettingsService>()));
            services.RegisterLazySingleton<IAxisCloudService>(() => new AxisCloudService());
            services.RegisterLazySingleton<ITranslationService>(() => new TranslationService());
            services.RegisterLazySingleton<IExplanationService>(() => new ExplanationService());            
            services.RegisterLazySingleton<ILoggerService>(() => new LoggerService());
            services.RegisterLazySingleton<IValidationService>(() => new ValidationService(resolver.GetRequiredService<ISettingsService>()));
            services.Register<IReportsService>(() => new BulgarianReportsService());
            services.Register<IExchangeService>(() => new ExchangeService(
                resolver.GetRequiredService<ISettingsService>(),
                resolver.GetRequiredService<IExchangesRepository>(), 
                resolver.GetRequiredService<ISearchData>(),
                resolver.GetRequiredService<IOperationHeaderRepository>(),
                resolver.GetRequiredService<IVATsRepository>(),
                resolver.GetRequiredService<IPaymentTypesRepository>(),
                resolver.GetRequiredService<ITranslationService>(),
                resolver.GetRequiredService<ILoggerService>(),
                resolver.GetRequiredService<IItemRepository>(),
                resolver.GetRequiredService<IPartnerRepository>(),
                resolver.GetRequiredService<IItemsGroupsRepository>(),
                resolver.GetRequiredService<IPartnersGroupsRepository>()));

            services.Register<IZipService>(() => new ZipService());

            switch (AvaloniaLocator.Current?.GetService<IRuntimePlatform>()?.GetRuntimeInfo().OperatingSystem)
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

            RegisterViewModels(services, resolver);
            RegisterRepositories(services, resolver);
        }

        private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register(() => new MainWindowViewModel());
            services.Register(() => new SaleViewModel());
            services.Register(() => new InvoiceViewModel());
            services.Register(() => new ProformInvoiceViewModel());
            services.Register(() => new DebitNoteViewModel());
            services.Register(() => new CreditNoteViewModel());
            services.Register(() => new CashRegisterViewModel());
            services.Register(() => new ExchangeViewModel());
            services.Register(() => new ReportsViewModel(
                resolver.GetRequiredService<IReportsService>(), 
                resolver.GetRequiredService<ISerializationService>()));
            services.Register(() => new SettingsViewModel());
        }

        private static void RegisterRepositories(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
        {
            services.Register(() => new DatabaseContext(Configurations.AppConfiguration.GetDatabaseOptions()));
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
    }
}
