using DataBase.Entities.Exchanges;
using DataBase.Enums;
using Microinvest.CommonLibrary.Enums;
using Microinvest.ExchangeDataService.Models.Auditor;
using System;
using System.Collections.Generic;
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
        /// Checks whether data was imported.
        /// </summary>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns true if data was imported; otherwise returns false.</returns>
        /// <date>21.07.2022.</date>
        Task<(long ImpotedOperAcct, int OperationHeaderId)> GetImportedData(string appName, string appKey, long acct, EOperTypes operType);

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

        /// <summary>
        /// Adds new record to the Exchange table.
        /// </summary>
        /// <param name="headerAcct">Acct of record of an OperationHeader table.</param>
        /// <param name="headerOperType">Operation type of record of an OperationHeader table.</param>
        /// <param name="direction">Direction of an exchange.</param>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns false if record wasn't added to database; otherwise returns true.</returns>
        /// <date>15.07.2022.</date>
        Task<bool> AddNewRecordAsync(long headerAcct, EOperTypes headerOperType, EExchangeDirections direction, string appName, string appKey, long acct, EOperTypes operType);

        /// <summary>
        /// Gets id of the exported operations. 
        /// </summary>
        /// <param name="operTypes">Types of operations to search exported data into the database.</param>
        /// <param name="dateFrom">Start date to search exported data into the database.</param>
        /// <param name="dateTo">End date to search exported data into the database.</param>
        /// <param name="acctFrom">Start acct to search exported data into the database.</param>
        /// <param name="acctTo">End acct to search exported data into the database.</param>
        /// <param name="appName">Name of app to exchange to search exported data into the database.</param>
        /// <param name="appKey">Key of app to exchange to search exported data into the database.</param>
        /// <returns>Returns list with id of the exported operations.</returns>
        /// <date>14.07.2022.</date>
        Task<List<int>> GetExportedRecordsIdAsync(EOperTypes[] operTypes, DateTime dateFrom, DateTime dateTo, long acctFrom, long acctTo, string appName, string appKey);

        /// <summary>
        /// Gets records to generate export file for Delta Pro.
        /// </summary>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="appName">Name of app to exchange to search exported data into the database.</param>
        /// <param name="appKey">Key of app to exchange to search exported data into the database.</param>
        /// <returns>Returns all records to prepare data for export to Delta Pro if "appName" is string.Empty; otherwise, it returns only previously unexported entries.</returns>
        /// <date>15.07.2022.</date>
        Task<List<Microinvest.ExchangeDataService.Models.DeltaPro.OperationRowModel>> GetRecordsForDeltaProAsync(DateTime dateFrom, DateTime dateTo, long acctFrom, long acctTo, string appName = "", string appKey = "");

        /// <summary>
        /// Gets records to generate export file for NAP.
        /// </summary>
        /// <param name="date">Date to search data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <returns>Returns all sales and refunds to prepare data for export to NAP.</returns>
        /// <date>15.07.2022.</date>
        Task<(List<Operation> Sales, List<Refund> Refunds)> GetRecordsForNAPAsync(DateTime date, long acctFrom, long acctTo);

        /// <summary>
        /// Gets records to generate export file for some application (exported file will be with 5 columns (acct, data of item, price, qty and data of partner )).
        /// </summary>
        /// <param name="operType">Type of operations to search exported data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <returns>Returns all records to prepare data for some application.</returns>
        /// <date>18.07.2022.</date>
        Task<List<Entities.OperationHeader.OperationHeader>> GetRecordsForUnidentifiedAppAsync(EOperTypes operType, long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Gets records to generate export file for Warehouse Sklad Pro.
        /// </summary>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <param name="appName">Name of app to exchange to search exported data into the database.</param>
        /// <param name="appKey">Key of app to exchange to search exported data into the database.</param>
        /// <returns>Returns all records to prepare data for Warehouse Sklad Pro if "appName" is string.Empty; otherwise, it returns only previously unexported entries.</returns>
        /// <date>18.07.2022.</date>
        Task<(List<Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel> Operations, List<Microinvest.ExchangeDataService.Models.WarehousePro.PartnerModel> Partners, List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel> Items)> GetRecordsForWarehouseSkladPro(long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo, string appName = "", string appKey = "");
    }
}
