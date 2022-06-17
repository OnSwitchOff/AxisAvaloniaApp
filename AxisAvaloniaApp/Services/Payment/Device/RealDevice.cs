using Microinvest.CommonLibrary.Enums;
using Microinvest.DeviceService;
using Microinvest.DeviceService.Helpers;
using Microinvest.DeviceService.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Logger;

namespace AxisAvaloniaApp.Services.Payment.Device
{
    /// <summary>
    /// Describes payment service.
    /// </summary>
    public class RealDevice : IDevice
    {
        private readonly ILoggerService loggerService;
        private FiscalPrinterService fiscalPrinter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealDevice"/> class.
        /// </summary>
        /// <param name="settings">Settings of the app.</param>
        public RealDevice(ISettingsService settings)
        {
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
            InitializeDeviceSettings(settings);
        }

        /// <summary>
        /// Service to print receipts.
        /// </summary>
        /// <date>15.06.2022.</date>
        private FiscalPrinterService FiscalPrinter
        {
            get
            {
                if (fiscalPrinter == null)
                {
                    throw new Exception("Fiscal printer service is not initialized!");
                }

                return fiscalPrinter;
            }
        }

        /// <summary>
        /// Gets number of receipt.
        /// </summary>
        /// <date>17.03.2022.</date>
        public string ReceiptNumber
        {
            get => this.FiscalPrinter.ReceiptNumber;
        }

        /// <summary>
        /// Gets serial number of fiscal device.
        /// </summary>
        /// <date>17.03.2022.</date>
        public string FiscalPrinterSerialNumber
        {
            get => this.FiscalPrinter.SerialNumber;
        }

        /// <summary>
        /// Gets memory number of fiscal device.
        /// </summary>
        /// <date>17.03.2022.</date>
        public string FiscalPrinterMemoryNumber
        {
            get => this.FiscalPrinter.FiscalMemoryNumber;
        }

        /// <summary>
        /// Initialize settings of device.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        /// <date>17.03.2022.</date>
        private void InitializeDeviceSettings(ISettingsService settings)
        {
            IDeviceSettings deviceSettings = new DeviceSettings(settings);
            try
            {
                this.fiscalPrinter = FiscalPrinterService.Instance(deviceSettings, settings.AppLanguage.Convert());
                this.fiscalPrinter.UniqueSaleNumber = settings.UniqueSaleNumber;

                if ((bool)settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIsUsed])
                {
                    this.fiscalPrinter.POSTerminal = POSTerminalService.Instance(
                        deviceSettings, 
                        settings.AppLanguage.Convert(),
                        settings.Country.Convert());
                }
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(InitializeDeviceSettings));
            }
        }

        /// <summary>
        /// Pay order by POS terminal (if it is used) and print fiscal receipt.
        /// </summary>
        /// <param name="products">Products list to sale.</param>
        /// <param name="paymentType">Payment type.</param>
        /// <param name="receivedCash">Amount of money has paid by customer.</param>
        /// <returns>
        /// Returns FiscalExecutionResult with:
        /// - empty ResultException and AdditionalData properties if the print receipt has been success;
        /// - initialized ResultException and AdditionalData (DialogResult.Abort/DialogResult.Ignore) properties if the print receipt has been unsuccess.
        /// </returns>
        /// <date>17.03.2022.</date>
        public async Task<FiscalExecutionResult> PayOrderAsync(ObservableCollection<Models.OperationItemModel> products, EPaymentTypes paymentType, decimal receivedCash)
        {
            switch (paymentType)
            {
                case EPaymentTypes.Cash:
                case EPaymentTypes.ElectronicPoints:
                case EPaymentTypes.Voucher:
                case EPaymentTypes.Other1:
                case EPaymentTypes.Other2:
                case EPaymentTypes.Other3:
                case EPaymentTypes.Other4:
                case EPaymentTypes.Card:
                    return await this.fiscalPrinter.SendFiscalReceiptAsync(paymentType, products.ParseToList(), Math.Round(receivedCash, 2));
                default:
                    return new FiscalExecutionResult()
                    {
                        ResultException = new Exception("Unknown payment type!"),
                    };
            }
        }

        /// <summary>
        /// Checks whether fiscal device is connected.
        /// </summary>
        /// <param name="device">Settings of the fiscal device.</param>
        /// <param name="language">Language of the app.</param>
        /// <param name="country">Country in which the app is used.</param>
        /// <returns>Returns true if device is connected; otherwise returns false.</returns>
        /// <date>16.06.2022.</date>
        public static async Task<(bool IsConnected, string error)> FiscalDeviceIsConnectedAsync(Models.DeviceSettingsModel device, ELanguages language, ECountries country)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var printer = FiscalPrinterService.Instance(new DeviceSettings(device, null), language.Convert(), country.Convert());
                    if (!printer.IsConnected)
                    {
                        Translation.ITranslationService translationService = Splat.Locator.Current.GetRequiredService<Translation.ITranslationService>();

                        return (false, translationService.Localize("msgUnSuccessfulConnection"));                        
                    }
                }
                catch (NullReferenceException nre)
                {
                    Translation.ITranslationService translationService = Splat.Locator.Current.GetRequiredService<Translation.ITranslationService>();

                    return (false, translationService.Localize("msgSomeDeviceSettingsAreNotFilled"));
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                }

                return (true, string.Empty);
            });
        }

        /// <summary>
        /// Checks whether POS terminal is connected.
        /// </summary>
        /// <param name="device">Settings of the POS terminal.</param>
        /// <param name="language">Language of the app.</param>
        /// <param name="country">Country in which the app is used.</param>
        /// <returns>Returns true if device is connected; otherwise returns false.</returns>
        /// <date>16.06.2022.</date>
        public static async Task<(bool IsConnected, string error)> POSTerminalIsConnectedAsync(Models.DeviceSettingsModel device, ELanguages language, ECountries country)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var pOS = POSTerminalService.Instance(new DeviceSettings(null, device), language.Convert(), country.Convert());
                    if (!pOS.IsConnected)
                    {
                        Translation.ITranslationService translationService = Splat.Locator.Current.GetRequiredService<Translation.ITranslationService>();

                        return (false, translationService.Localize("msgUnSuccessfulConnection"));
                    }
                }
                catch (NullReferenceException nre)
                {
                    Translation.ITranslationService translationService = Splat.Locator.Current.GetRequiredService<Translation.ITranslationService>();

                    return (false, translationService.Localize("msgSomeDeviceSettingsAreNotFilled"));
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                }

                return (true, string.Empty);
            });
        }
    }
}
