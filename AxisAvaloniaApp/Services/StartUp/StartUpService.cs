using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.AxisCloud;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Payment.Device;
using AxisAvaloniaApp.Services.Scanning;
using AxisAvaloniaApp.Services.Settings;
using DataBase.Entities.VATGroups;
using DataBase.Repositories.OperationHeader;
using Microinvest.CommonLibrary.Enums;

namespace AxisAvaloniaApp.Services.StartUp
{
    public class StartUpService : IStartUpService
    {
        private readonly ISettingsService settings;
        private readonly IScanningData scanningService;
        private readonly IPaymentService paymentService;
        private readonly IAxisCloudService axisCloudService;
        private readonly ILoggerService loggerService;
        private readonly IOperationHeaderRepository headerRepository;

        //private UIElement shell = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpService"/> class.
        /// </summary>
        /// <param name="settings">Settings of the app.</param>
        /// <param name="scanningService">Service to read data from COM scanner.</param>
        /// <param name="paymentService">Service to print receipt.</param>
        /// <param name="axisCloudService">Service to connect with AxicCloud app.</param>
        /// <param name="loggerService">Service to log errors.</param>
        public StartUpService(ISettingsService settings, IScanningData scanningService, IPaymentService paymentService, IAxisCloudService axisCloudService, ILoggerService loggerService)
        {
            this.settings = settings;
            this.scanningService = scanningService;
            this.paymentService = paymentService;
            this.axisCloudService = axisCloudService;
            this.loggerService = loggerService;

            headerRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
        }

        public async Task ActivateAsync()
        {
            if (!Configurations.AppConfiguration.IsDatabaseExist)
            {
                await InitializeAsync();
            }

            await StartupAsync();
        }

        private async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Avalonia.Media.Imaging.Bitmap logo = new Avalonia.Media.Imaging.Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri("avares://AxisAvaloniaApp/Assets/AxisIcon.ico")));
                    logo.Save(Configurations.AppConfiguration.LogoPath);
                    
                    WriteDefaultValuesIntoDatabase();
                    
                    System.IO.File.Create(Configurations.AppConfiguration.LogFilePath);
                }
                catch (Exception ex)
                {
                    loggerService.RegisterError(this, ex, nameof(InitializeAsync));
                }
            });
        }

        private async Task StartupAsync()
        {
            await Task.Run(async () =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(settings.COMScannerSettings[Enums.ESettingKeys.ComPort]) &&
                    !settings.COMScannerSettings[Enums.ESettingKeys.ComPort].ToString().Equals("strNotActive"))
                    {
                        scanningService.StartCOMScanner(settings.COMScannerSettings[Enums.ESettingKeys.ComPort]);
                    }

                    if ((bool)settings.AxisCloudSettings[Enums.ESettingKeys.DeviceIsUsed])
                    {
                        axisCloudService.StartServiceAsync((int)settings.AxisCloudSettings[Enums.ESettingKeys.ComPort], settings);
                    }
                }
                catch (Exception ex)
                {
                    loggerService.RegisterError(this, ex, nameof(StartupAsync) + " (COMScanner/AxisCloud)");
                }

                try
                {
                    if ((bool)settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIsUsed])
                    {
                        paymentService.SetPaymentTool(new RealDevice(settings));
                    }
                    else
                    {
                        paymentService.SetPaymentTool(new NoDevice(settings));
                    }

                    int uSNFromDatabase = await headerRepository.GetNextSaleNumberAsync(paymentService.FiscalDevice.FiscalPrinterSerialNumber);
                    settings.UniqueSaleNumber = Math.Max((int)settings.AppSettings[Enums.ESettingKeys.UniqueSaleNumber], uSNFromDatabase);
                }
                catch (Exception e)
                {
                    loggerService.RegisterError(this, e, nameof(StartupAsync) + " (Fiscal printer)");
                }
            });
        }

        /// <summary>
        /// Insert default values into the database.
        /// </summary>
        /// <date>17.06.2022.</date>
        private async void WriteDefaultValuesIntoDatabase()
        {
            Translation.ITranslationService translationService = Splat.Locator.Current.GetRequiredService<Translation.ITranslationService>();

            DataBase.Repositories.ItemsGroups.IItemsGroupsRepository itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.ItemsGroups.IItemsGroupsRepository>();
            DataBase.Entities.ItemsGroups.ItemsGroup itemsGroup = DataBase.Entities.ItemsGroups.ItemsGroup.Create(
                "-1",
                translationService.Localize("strBaseGroup"),
                0);
            await itemsGroupsRepository.AddGroupAsync(itemsGroup);

            DataBase.Repositories.VATGroups.IVATsRepository vATsRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.VATGroups.IVATsRepository>();
            System.Collections.Generic.List<VATGroup> vATGroups = new System.Collections.Generic.List<VATGroup>();
            if (settings.Country == ECountries.Bulgaria)
            {
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroupA"), 0));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroupB"), 20));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroupC"), 20));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroupD"), 9));
            }
            if (settings.Country == ECountries.Russia)
            {
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup1"), 20));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup2"), 10));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup3"), 0));
            }
            if (settings.Country == ECountries.Ukraine)
            {
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup1"), 20));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup2"), 0));
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup3"), 7));
            }
            if (settings.Country == ECountries.Georgia)
            {
                vATGroups.Add(VATGroup.Create(translationService.Localize("strVATGroup1"), 18));
            }
            await vATsRepository.AddVATGroupsAsync(vATGroups);

            DataBase.Repositories.Items.IItemRepository itemRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.Items.IItemRepository>();
            DataBase.Entities.Items.Item item = DataBase.Entities.Items.Item.Create(
                "1",
                translationService.Localize("strBaseGoods"),
                "",
                translationService.Localize("strMeasureItem"),
                itemsGroup,
                vATGroups[0], 
                EItemTypes.Standard, 
                new System.Collections.Generic.List<DataBase.Entities.ItemsCodes.ItemCode>());
            item.Status = ENomenclatureStatuses.All;
            await itemRepository.AddItemAsync(item);
            

            DataBase.Repositories.PartnersGroups.IPartnersGroupsRepository partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.PartnersGroups.IPartnersGroupsRepository>();
            DataBase.Entities.PartnersGroups.PartnersGroup partnersGroup = DataBase.Entities.PartnersGroups.PartnersGroup.Create(
                "-1",
                translationService.Localize("strBaseGroup"), 
                0);
            await partnersGroupsRepository.AddGroupAsync(partnersGroup);

            DataBase.Repositories.Partners.IPartnerRepository partnerRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.Partners.IPartnerRepository>();
            DataBase.Entities.Partners.Partner partner = DataBase.Entities.Partners.Partner.Create(
                translationService.Localize("strBasePartner"), 
                translationService.Localize("strBasePartner"), 
                "", 
                "", 
                "", 
                "", 
                "", 
                "", 
                "", 
                "", 
                "", 
                "", 
                partnersGroup);
            partner.Status = ENomenclatureStatuses.All;
            await partnerRepository.AddPartnerAsync(partner);
        }
    }
}
