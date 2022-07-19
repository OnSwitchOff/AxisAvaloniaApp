using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AxisAvaloniaApp.Configurations;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Activation;
using AxisAvaloniaApp.Services.Activation.ResponseModels;
using AxisAvaloniaApp.Services.AxisCloud;
using AxisAvaloniaApp.Services.Crypto;
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
        private const int OFFLINE_CHECK_INTERVAL = 60 * 60;
        private const int OFFLINE_LIMIT_INTERVAL = 60 * 60 * 24 * 3;
        private const int ACTIVATION_CHECK_INTERVAL = 60 * 60 * 24; 

        private readonly ISettingsService settings;
        private readonly IScanningData scanningService;
        private readonly IPaymentService paymentService;
        private readonly IAxisCloudService axisCloudService;
        private readonly ILoggerService loggerService;
        private readonly ISearchData searchService;
        private readonly IOperationHeaderRepository headerRepository;
        private readonly IZipService zipService;
        private readonly IActivationService activationService;
        private readonly ICryptoService cryptoService;

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
            cryptoService = Splat.Locator.Current.GetRequiredService<ICryptoService>();
        }

        public async Task ActivateAsync(bool isFirstRun)
        {
            if (isFirstRun)
            {
                await InitializeAsync();
            }
            else
            {
                activationService.SoftwareID = settings.AppSettings[Enums.ESettingKeys.SoftwareID].Value;
            }

            if (settings.AppSettings[Enums.ESettingKeys.BackUpOption].Value == "1")
            {
                BackUp();
            }

            await CheckStartupEnvironment(isFirstRun);

            await StartupAsync();
        }

        private async Task<bool> CheckStartupEnvironment(bool isFirstRun)
        {
            if(CheckNetworkConnection())
            {
                ResetOfflinecounter();
                ActivationResponse.GetStatusModel getStatus = await activationService.GetStatusModel();
                VersionResponse.GetLastVersion getLastVersion = await activationService.GetLastVersionModel();
                DateTime expirDate = DateTime.ParseExact(getStatus.Expirationdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                settings.IsActiveLicense = DateTime.Compare(expirDate, DateTime.Now) > 0;
                StartActivationChecker();
            }
            else
            {
                if (isFirstRun)
                {
                    Environment.Exit(-218);
                }
                StartOfflineTimer();
            }
            return true;
        }

        private void StartActivationChecker()
        {
            Avalonia.Threading.DispatcherTimer ActivationCheckerTimer = new Avalonia.Threading.DispatcherTimer();
            ActivationCheckerTimer.Interval = TimeSpan.FromSeconds(ACTIVATION_CHECK_INTERVAL);
            ActivationCheckerTimer.Tick += ActivationCheckerTimer_Tick;
            ActivationCheckerTimer.Start();
        }

        private async void ActivationCheckerTimer_Tick(object? sender, EventArgs e)
        {
            ActivationResponse.GetStatusModel getStatus = await activationService.GetStatusModel();
            DateTime expirDate = DateTime.ParseExact(getStatus.Expirationdate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            settings.IsActiveLicense = DateTime.Compare(expirDate, DateTime.Now) > 0;
            if (!settings.IsActiveLicense)
            {
                await loggerService.ShowDialog("Activate me, please!", icon: UserControls.MessageBox.EButtonIcons.Info);
                Environment.Exit(-1);
            }
        }

        private void ResetOfflinecounter()
        {
            settings.AppSettings[Enums.ESettingKeys.SoftwareVersion].Value = cryptoService.Encrypt("0");
            settings.UpdateSettings(Enums.ESettingGroups.App);
        }

        private void StartOfflineTimer()
        {
            int currentInterval = (StartUpAppWorkModel)settings.AppSettings[Enums.ESettingKeys.SoftwareVersion].Value;
            int remainingInterval = OFFLINE_LIMIT_INTERVAL - currentInterval < 0 ? 0 : OFFLINE_LIMIT_INTERVAL - currentInterval;
            App.OfflineTimer.Interval = TimeSpan.FromSeconds(remainingInterval);
            App.OfflineTimer.Tick += OfflineTimer_Tick;
            App.OfflineTimer.Start();

            Avalonia.Threading.DispatcherTimer offlineTimeRefresherTimer = new Avalonia.Threading.DispatcherTimer();
            offlineTimeRefresherTimer.Interval = TimeSpan.FromSeconds(OFFLINE_CHECK_INTERVAL);
            offlineTimeRefresherTimer.Tick += OfflineTimeRefresherTimer_Tick;
            offlineTimeRefresherTimer.Start();
        }
        private async void OfflineTimer_Tick(object? sender, EventArgs e)
        {
            App.OfflineTimer.Tick -= OfflineTimer_Tick;
            App.OfflineTimer.Stop();
            settings.AppSettings[Enums.ESettingKeys.SoftwareVersion].Value = cryptoService.Encrypt(OFFLINE_LIMIT_INTERVAL.ToString());
            settings.UpdateSettings(Enums.ESettingGroups.App);

            await loggerService.ShowDialog("Offline limit - 0!", icon: UserControls.MessageBox.EButtonIcons.Stop);

            Environment.Exit(-1);
        }

        private void OfflineTimeRefresherTimer_Tick(object? sender, EventArgs e)
        {
            int currentInterval = (StartUpAppWorkModel)settings.AppSettings[Enums.ESettingKeys.SoftwareVersion].Value + OFFLINE_CHECK_INTERVAL;
            settings.AppSettings[Enums.ESettingKeys.SoftwareVersion].Value = cryptoService.Encrypt(currentInterval.ToString());
            settings.UpdateSettings(Enums.ESettingGroups.App);
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
                    settings.AppSettings[Enums.ESettingKeys.SoftwareID].Value = activationService.GenerateUserAgentID();
                    settings.UpdateSettings(Enums.ESettingGroups.App);

                    Microinvest.PDFCreator.MicroinvestPdfDocument pdf = new Microinvest.PDFCreator.MicroinvestPdfDocument();
                    pdf.DefaultHeaderImage.Save(Configurations.AppConfiguration.LogoPath);
                    pdf.DefaultHeaderImage.Save(Configurations.AppConfiguration.DocumentHeaderPath);
                    pdf.DefaultFooterImage.Save(Configurations.AppConfiguration.DocumentFooterPath);
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
