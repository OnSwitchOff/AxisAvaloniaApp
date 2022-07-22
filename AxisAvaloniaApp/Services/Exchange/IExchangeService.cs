using Microinvest.ExchangeDataService.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Exchange
{
    public delegate void ExportDataStatus(string status);

    public interface IExchangeService
    {
        /// <summary>
        /// Adds or removes event that occurs when the status of export is changed.
        /// </summary>
        /// <date>19.07.2022.</date>
        event Action<string> ExchangeDataStatus;

        /// <summary>
        /// Adds or removes event that occurs when we need to get currency rate.
        /// </summary>
        /// <date>19.07.2022.</date>
        event Func<string, Task<double>> GetCurrencyRateAsync;
        
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
        Task<string> GetDataForExportAsync(EExchanges app, long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo, string appName, string appKey);

        /// <summary>
        /// Parses and writes down import data to database.
        /// </summary>
        /// <param name="app">An application from which data will be imported.</param>
        /// <param name="filePath">Path to file with import data.</param>
        /// <returns>Returns true if data was parsed and written down to database successfully; otherwise returns false.</returns>
        /// <date>19.07.2022.</date>
        Task<bool> SaveImportedData(EExchanges app, string filePath);
    }
}
