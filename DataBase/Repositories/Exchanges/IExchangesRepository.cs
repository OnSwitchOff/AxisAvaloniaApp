using DataBase.Entities.Exchanges;
using DataBase.Enums;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Threading.Tasks;

namespace DataBase.Repositories.Exchanges
{
    public interface IExchangesRepository
    {
        /// <summary>
        /// Checks if record is exist.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns true, id of record and acct if record exist; otherwise returns false, 0, 0.</returns>
        /// <date>16.06.2022.</date>
        Task<(bool isExist, int id, int acct)> CheckRecordAsync(EExchangeDirections exchangeType, string appName, string appKey);

        /// <summary>
        /// Gets record in according to parameters.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns Exchange object if record exist; otherwise returns null.</returns>
        /// <date>16.06.2022.</date>
        Task<Exchange> GetRecordAsync(EExchangeDirections exchangeType, string appName, string appKey);

        /// <summary>
        /// Checks whether data was exported to NAP. 
        /// </summary>
        /// <param name="date">Date to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        Task<bool> DataWasExportedToNAP(DateTime date);

        /// <summary>
        /// Checks whether data was exported to unidentified app.
        /// </summary>
        /// <param name="operType">Type of the operation to search data to the database.</param>
        /// <param name="from">Start date to search data to the database.</param>
        /// <param name="to">End date to search data to the database.</param>
        /// <param name="acctFrom">Start acct to search data to the database.</param>
        /// <param name="acctTo">End acct to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        Task<bool> DataWasExportedToUnidentifiedApp(EOperTypes operType, DateTime from, DateTime to, long acctFrom, long acctTo);

        /// <summary>
        /// Checks whether data was exported to Warehouse Sklad Pro.
        /// </summary>
        /// <param name="from">Start date to search data to the database.</param>
        /// <param name="to">End date to search data to the database.</param>
        /// <param name="acctFrom">Start acct to search data to the database.</param>
        /// <param name="acctTo">End acct to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        Task<bool> DataWasExportedToWarehouseSkladPro(DateTime from, DateTime to, long acctFrom, long acctTo);

        /// <summary>
        /// Checks whether data was imported.
        /// </summary>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns true if data was imported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        Task<bool> DataWasImported(string appName, string appKey, long acct, EOperTypes operType);

        /// <summary>
        /// Adds new record to the Exchange table.
        /// </summary>
        /// <param name="operationHeaderId">Id of record of an OperationHeader table.</param>
        /// <param name="direction">Direction of an exchange.</param>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns false if record wasn't added to database; otherwise returns true.</returns>
        /// <date>13.07.2022.</date>
        Task<bool> AddNewRecordAsync(int operationHeaderId, EExchangeDirections direction, string appName, string appKey, long acct, EOperTypes operType);
    }
}
