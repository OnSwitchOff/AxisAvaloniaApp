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

namespace AxisAvaloniaApp.Services.Payment.Device
{
    /// <summary>
    /// Describes payment service.
    /// </summary>
    public class RealDevice : IDevice
    {
        private FiscalPrinterService fiscalPrinter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealDevice"/> class.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        public RealDevice(ISettingsService settings)
        {
            this.InitializeDeviceSettings(settings);
        }

        /// <summary>
        /// Gets number of receipt.
        /// </summary>
        /// <date>17.03.2022.</date>
        public string ReceiptNumber
        {
            get => this.fiscalPrinter.ReceiptNumber;
        }

        /// <summary>
        /// Gets serial number of fiscal device.
        /// </summary>
        /// <date>17.03.2022.</date>
        public string FiscalPrinterSerialNumber
        {
            get => this.fiscalPrinter.SerialNumber;
        }

        /// <summary>
        /// Gets memory number of fiscal device.
        /// </summary>
        /// <date>17.03.2022.</date>
        public string FiscalPrinterMemoryNumber
        {
            get => this.fiscalPrinter.FiscalMemoryNumber;
        }

        /// <summary>
        /// Initialize settings of device.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        /// <date>17.03.2022.</date>
        public void InitializeDeviceSettings(ISettingsService settings)
        {
            IDeviceSettings deviceSettings = new DeviceSettings(settings);
            this.fiscalPrinter = FiscalPrinterService.Instance(deviceSettings, settings.AppLanguage.Convert());
            this.fiscalPrinter.UniqueSaleNumber = settings.UniqueSaleNumber;

            if (this.fiscalPrinter.POSTerminal == null && (bool)settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIsUsed])
            {
                this.fiscalPrinter.POSTerminal = POSTerminalService.Instance(deviceSettings, settings.AppLanguage.Convert());
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
    }
}
