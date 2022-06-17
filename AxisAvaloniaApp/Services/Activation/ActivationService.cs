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

namespace AxisAvaloniaApp.Services.Activation
{
    public class ActivationService : IActivationService
    {
        private readonly ISettingsService settings;
        private readonly IScanningData scanningService;
        private readonly IPaymentService paymentService;
        private readonly IAxisCloudService axisCloudService;
        private readonly ILoggerService loggerService;

        //private UIElement shell = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationService"/> class.
        /// </summary>
        /// <param name="settings">Settings of the app.</param>
        /// <param name="scanningService">Service to read data from COM scanner.</param>
        /// <param name="paymentService">Service to print receipt.</param>
        /// <param name="axisCloudService">Service to connect with AxicCloud app.</param>
        /// <param name="loggerService">Service to log errors.</param>
        public ActivationService(ISettingsService settings, IScanningData scanningService, IPaymentService paymentService, IAxisCloudService axisCloudService, ILoggerService loggerService)
        {
            this.settings = settings;
            this.scanningService = scanningService;
            this.paymentService = paymentService;
            this.axisCloudService = axisCloudService;
            this.loggerService = loggerService;
        }

        public async Task ActivateAsync()
        {
            await InitializeAsync();

            await StartupAsync();
        }

        private async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!System.IO.File.Exists(Configurations.AppConfiguration.LogoPath))
                    {
                        Avalonia.Media.Imaging.Bitmap logo = new Avalonia.Media.Imaging.Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri("avares://AxisAvaloniaApp/Assets/AxisIcon.ico")));
                        logo.Save(Configurations.AppConfiguration.LogoPath);
                    }

                    if (!System.IO.File.Exists(Configurations.AppConfiguration.LogFilePath))
                    {
                        System.IO.File.Create(Configurations.AppConfiguration.LogFilePath);
                    }
                }
                catch (Exception ex)
                {
                    loggerService.RegisterError(this, ex, nameof(InitializeAsync));
                }
            });
        }

        private async Task StartupAsync()
        {
            await Task.Run(() =>
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
                }
                catch (Exception e)
                {
                    loggerService.RegisterError(this, e, nameof(StartupAsync) + " (Fiscal printer)");
                }
            });
        }

        private void WriteDefaultValuesIntoDatabase()
        {
            Translation.ITranslationService translationService = Splat.Locator.Current.GetRequiredService<Translation.ITranslationService>();

            DataBase.Repositories.ItemsGroups.IItemsGroupsRepository itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.ItemsGroups.IItemsGroupsRepository>();
            DataBase.Entities.ItemsGroups.ItemsGroup itemsGroup = DataBase.Entities.ItemsGroups.ItemsGroup.Create(
                "-1",
                translationService.Localize("strBaseGroup"),
                0);
            itemsGroupsRepository.AddGroupAsync(itemsGroup);

            DataBase.Repositories.VATGroups.IVATsRepository vATsRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.VATGroups.IVATsRepository>();

            DataBase.Repositories.Items.IItemRepository itemRepository = Splat.Locator.Current.GetRequiredService<DataBase.Repositories.Items.IItemRepository>();
            //DataBase.Entities.Items.Item item = DataBase.Entities.Items.Item.Create(
            //    "1", 
            //    translationService.Localize("strBaseGoods"), 
            //    "", 
            //    translationService.Localize("strMeasureItem"),
            //    itemsGroup,);
            //itemRepository.AddItemAsync(item);
        }
    }
}
