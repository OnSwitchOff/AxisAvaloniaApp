﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AxisAvaloniaApp.Configurations;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Services.Activation.ResponseModels;
using AxisAvaloniaApp.Services.AxisCloud;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Payment.Device;
using AxisAvaloniaApp.Services.Scanning;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Zip;
using DataBase.Entities.VATGroups;
using DataBase.Repositories.OperationHeader;
using Microinvest.CommonLibrary.Enums;
using Newtonsoft.Json;

namespace AxisAvaloniaApp.Services.StartUp
{
    public class StartUpService : IStartUpService
    {
        private readonly ISettingsService settings;
        private readonly IScanningData scanningService;
        private readonly IPaymentService paymentService;
        private readonly IAxisCloudService axisCloudService;
        private readonly ILoggerService loggerService;
        private readonly ISearchData searchService;
        private readonly IOperationHeaderRepository headerRepository;
        private readonly IZipService zipService;
        private readonly IActivationService activationService;

        //private UIElement shell = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpService"/> class.
        /// </summary>
        /// <param name="settings">Settings of the app.</param>
        /// <param name="scanningService">Service to read data from COM scanner.</param>
        /// <param name="paymentService">Service to print receipt.</param>
        /// <param name="axisCloudService">Service to connect with AxicCloud app.</param>
        /// <param name="loggerService">Service to log errors.</param>
        public StartUpService(
            ISettingsService settings, 
            IScanningData scanningService, 
            IPaymentService paymentService, 
            IAxisCloudService axisCloudService, 
            ILoggerService loggerService,
            ISearchData searchService,
            IOperationHeaderRepository headerRepository)
        {
            this.settings = settings;
            this.scanningService = scanningService;
            this.paymentService = paymentService;
            this.axisCloudService = axisCloudService;
            this.loggerService = loggerService;
            this.searchService = searchService;
            this.headerRepository = headerRepository;

            zipService = Splat.Locator.Current.GetRequiredService<IZipService>();
            activationService = Splat.Locator.Current.GetRequiredService<IActivationService>();
        }

        public async Task ActivateAsync(bool isFirstRun)
        {
            if (isFirstRun)
            {
                await InitializeAsync();
            }

            BackUp();

            if (!await CheckStartupEnvironment(isFirstRun))
            {
                return;
            }

            await StartupAsync();
        }

        private async Task<bool> CheckStartupEnvironment(bool isFirstRun)
        {
            if(CheckNetworkConnection())
            {
                // получаем с сервера серийный номер нашего приложения
                var x1 = await activationService.GetStatus("8714536025");
                x1.EnsureSuccessStatusCode();
                var r1 = await x1.Content.ReadAsStringAsync();
                ActivationResponse.GetStatusModel getStatus = JsonConvert.DeserializeObject<ActivationResponse.GetStatusModel>(r1);

                // получаем с сервера данные о последней версии программы
                var x3 = await activationService.GetLastVersion();
                x3.EnsureSuccessStatusCode();
                var r3 = await x3.Content.ReadAsStringAsync();
                VersionResponse.GetLastVersion r = JsonConvert.DeserializeObject<VersionResponse.GetLastVersion>(r3);
            }
            else
            {
                if (isFirstRun)
                {
                    Environment.Exit(-218);
                }
            }
            return true;
        }

        /// <summary>
        /// Проверяет наличие интернета методом пинга google.com
        /// </summary>
        /// <returns>результат проверки</returns>
        private bool CheckNetworkConnection()
        {
            try
            {
                PingReply pingStatus = null;

                using (Ping ping = new Ping())
                {
                    pingStatus = ping.Send("google.com");
                }

                return pingStatus.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void BackUp()
        {
            string curentDate = DateTime.Now.ToString("dd.MM.yyyy(HH-mm)") + ".zip";
            string ZipName = Path.Combine(AppConfiguration.BackupFolderPath, curentDate);
            if (!zipService.CompressFileToZip(AppConfiguration.DatabaseFullName, ZipName, AppConfiguration.DatabaseShortName))
            {

            }
        }

        private async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Microinvest.PDFCreator.MicroinvestPdfDocument pdf = new Microinvest.PDFCreator.MicroinvestPdfDocument();
                    pdf.DefaultHeaderImage.Save(Configurations.AppConfiguration.LogoPath);
                    //using (Avalonia.Media.Imaging.Bitmap logo = new Avalonia.Media.Imaging.Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri("avares://AxisAvaloniaApp/Assets/AxisIcon.ico"))))
                    //{
                    //    logo.Save(Configurations.AppConfiguration.LogoPath);
                    //}
                    
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
                    searchService.InitializeSearchDataTool(settings.AppLanguage);

                    Microinvest.PDFCreator.Helpers.TranslationHelper.Language = settings.AppLanguage;

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

                    long uSNFromDatabase = await headerRepository.GetNextSaleNumberAsync(paymentService.FiscalDevice.FiscalPrinterSerialNumber);
                    settings.UniqueSaleNumber = (int)Math.Max((long)settings.AppSettings[Enums.ESettingKeys.UniqueSaleNumber], uSNFromDatabase);
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
            List<VATGroup> vATGroups = new List<VATGroup>();
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
                new List<DataBase.Entities.ItemsCodes.ItemCode>());
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

            DataBase.Repositories.PaymentTypes.IPaymentTypesRepository paymentTypesRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.PaymentTypes.IPaymentTypesRepository>();
            List<DataBase.Entities.PaymentTypes.PaymentType> payments = new List<DataBase.Entities.PaymentTypes.PaymentType>()
            {
                DataBase.Entities.PaymentTypes.PaymentType.Create(translationService.Localize("strInCash"), EPaymentTypes.Cash),
                DataBase.Entities.PaymentTypes.PaymentType.Create(translationService.Localize("strByAccount"), EPaymentTypes.Bank),
                DataBase.Entities.PaymentTypes.PaymentType.Create(translationService.Localize("strCard"), EPaymentTypes.Card),
                DataBase.Entities.PaymentTypes.PaymentType.Create(translationService.Localize("strVoucher"), EPaymentTypes.Voucher),
                DataBase.Entities.PaymentTypes.PaymentType.Create(translationService.Localize("strPoints"), EPaymentTypes.ElectronicPoints),
                DataBase.Entities.PaymentTypes.PaymentType.Create(string.Format("{0} {1}", translationService.Localize("strOtherPaymentType"), 1), EPaymentTypes.Other1),
                DataBase.Entities.PaymentTypes.PaymentType.Create(string.Format("{0} {1}", translationService.Localize("strOtherPaymentType"), 2), EPaymentTypes.Other2),
                DataBase.Entities.PaymentTypes.PaymentType.Create(string.Format("{0} {1}", translationService.Localize("strOtherPaymentType"), 3), EPaymentTypes.Other3),
                DataBase.Entities.PaymentTypes.PaymentType.Create(string.Format("{0} {1}", translationService.Localize("strOtherPaymentType"), 4), EPaymentTypes.Other4),
            };
            await paymentTypesRepository.AddPaymentTypesAsync(payments);
        }
    }
}
