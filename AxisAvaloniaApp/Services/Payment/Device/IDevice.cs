﻿using Microinvest.DeviceService.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Payment.Device
{
    /// <summary>
    /// Describes payment service.
    /// </summary>
    public interface IDevice : IDisposable
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
        Task<FiscalExecutionResult> PayOrderAsync(ObservableCollection<Models.OperationItemModel> products, Microinvest.CommonLibrary.Enums.EPaymentTypes paymentType, decimal receivedCash);

        Task<FiscalExecutionResult> PrintDailyReportZ();

        Task<FiscalExecutionResult> PrintDailyReportX();

        Task<FiscalExecutionResult> PrintDuplicateCheque();

        Task<FiscalExecutionResult> DepositeCash(decimal sum);

        Task<FiscalExecutionResult> WithdrawCash(decimal sum);

        Task<FiscalExecutionResult> CurrentMonthReport();

        Task<FiscalExecutionResult> LastMonthReport();

        Task<PinPadService.Interfaces.IBaseTransaction[]> ResetPOSterminal();

    }
}
