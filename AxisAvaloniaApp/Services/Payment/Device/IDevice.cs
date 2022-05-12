using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AxisAvaloniaApp.Services.Settings;

namespace AxisAvaloniaApp.Services.Payment.Device
{
    /// <summary>
    /// Describes payment service.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Gets number of receipt.
        /// </summary>
        /// <date>16.03.2022.</date>
        string ReceiptNumber { get; }

        /// <summary>
        /// Gets serial number of fiscal device.
        /// </summary>
        /// <date>16.03.2022.</date>
        string FiscalPrinterSerialNumber { get; }

        /// <summary>
        /// Gets memory number of fiscal device.
        /// </summary>
        /// <date>16.03.2022.</date>
        string FiscalPrinterMemoryNumber { get; }

        /// <summary>
        /// Initialize settings of device.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        /// <date>17.03.2022.</date>
        void InitializeDeviceSettings(ISettingsService settings);

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
        /// <date>16.03.2022.</date>
        Task<Microinvest.DeviceService.Helpers.FiscalExecutionResult> PayOrderAsync(ObservableCollection<Models.OperationItemModel> products, Microinvest.CommonLibrary.Enums.EPaymentTypes paymentType, decimal receivedCash);
    }
}
