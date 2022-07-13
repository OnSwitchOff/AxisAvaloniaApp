using Microinvest.ExchangeDataService.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.ExchangeService
{
    public delegate void ExportDataStatus(string status);

    public interface IExchangeService
    {
        /// <summary>
        /// Gets data from the database and prepares it for export
        /// </summary>
        /// <param name="app">An application to which data will be exported.</param>
        /// <param name="acctFrom">Start acct to search data to the database.</param>
        /// <param name="acctTo">End acct to search data to the database.</param>
        /// <param name="dateFrom">Start date to search data to the database.</param>
        /// <param name="dateTo">End date to search data to the database.</param>
        /// <returns>Returns String.Empty if data for export is absent; otherwise returns string with data for export.</returns>
        /// <date>13.07.2022.</date>
        Task<string> GetDataForExportAsync(EExchanges app, long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo);
    }
}
