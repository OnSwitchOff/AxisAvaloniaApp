using DataBase.Entities.Exchanges;
using DataBase.Enums;
using Microinvest.CommonLibrary.Enums;
using Microinvest.ExchangeDataService.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.Exchanges
{
    public class ExchangesRepository : IExchangesRepository
    {
        private readonly DatabaseContext databaseContext;
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangesRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ExchangesRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Checks if record is exist.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns true, id of record and acct if record exist; otherwise returns false, 0, 0.</returns>
        /// <date>16.06.2022.</date>
        public async Task<(bool isExist, int id, int acct)> CheckRecordAsync(EExchangeDirections exchangeType, string appName, string appKey)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Exchange record = databaseContext.Exchanges.
                    Where(r => r.ExchangeType == exchangeType && r.AppName.Equals(appName) && r.AppKey.Equals(appKey)).
                    FirstOrDefault();

                    if (record != null)
                    {
                        return (true, record.Id, (int)record.Acct);
                    }
                    else
                    {
                        return (false, 0, 0);
                    }
                }
            });
        }

        /// <summary>
        /// Gets record in according to parameters.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns Exchange object if record exist; otherwise returns null.</returns>
        /// <date>16.06.2022.</date>
        public async Task<Exchange> GetRecordAsync(EExchangeDirections exchangeType, string appName, string appKey)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(r => r.ExchangeType == exchangeType && r.AppName.Equals(appName) && r.AppKey.Equals(appKey)).
                    FirstOrDefault();
                }
            });
        }

        /// <summary>
        /// Checks whether data was exported to NAP. 
        /// </summary>
        /// <param name="date">Date to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasExportedToNAP(DateTime date)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => ex.ExchangeType == EExchangeDirections.Export && ex.AppName.Equals(EExchanges.ExportToNAP.ToString())).
                    Include(ex => ex.OperationHeader).
                    Where(ex => ex.OperationHeader.Date.Month == date.Month && ex.OperationHeader.Date.Year == date.Year).
                    FirstOrDefault() != null;
                }
            });
        }

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
        public async Task<bool> DataWasExportedToUnidentifiedApp(EOperTypes operType, DateTime from, DateTime to, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => ex.ExchangeType == EExchangeDirections.Export && ex.AppName.Equals(EExchanges.ExportToSomeApp.ToString())).
                    Include(ex => ex.OperationHeader).
                    Where(ex => 
                        ex.OperationHeader.OperType == operType && 
                        ex.OperationHeader.Date >= from && 
                        ex.OperationHeader.Date <= to.AddDays(1) && 
                        ex.OperationHeader.Acct >= acctFrom && 
                        ex.OperationHeader.Acct <= acctTo).
                    FirstOrDefault() != null;
                }
            });
        }

        /// <summary>
        /// Checks whether data was exported to Warehouse Sklad Pro.
        /// </summary>
        /// <param name="from">Start date to search data to the database.</param>
        /// <param name="to">End date to search data to the database.</param>
        /// <param name="acctFrom">Start acct to search data to the database.</param>
        /// <param name="acctTo">End acct to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasExportedToWarehouseSkladPro(DateTime from, DateTime to, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => ex.ExchangeType == EExchangeDirections.Export && ex.AppName.Equals(EExchanges.ExportToSomeApp.ToString())).
                    Include(ex => ex.OperationHeader).
                    Where(ex =>
                        (ex.OperationHeader.OperType == EOperTypes.Sale || ex.OperationHeader.OperType == EOperTypes.Refund || ex.OperationHeader.OperType == EOperTypes.Delivery) &&
                        ex.OperationHeader.Date >= from &&
                        ex.OperationHeader.Date <= to.AddDays(1) &&
                        ex.OperationHeader.Acct >= acctFrom &&
                        ex.OperationHeader.Acct <= acctTo).
                    FirstOrDefault() != null;
                }
            });
        }

        /// <summary>
        /// Checks whether data was imported.
        /// </summary>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns true if data was imported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasImported(string appName, string appKey, long acct, EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => 
                    ex.ExchangeType == EExchangeDirections.Import && 
                    ex.AppName.Equals(appName) &&
                    ex.AppKey.Equals(appKey) &&
                    ex.Acct == acct &&
                    ex.OperType == operType).
                    FirstOrDefault() != null;
                }
            });
        }

        public async Task<bool> AddNewRecord(int operationHeaderId, EExchangeDirections direction, string appName, string appKey, long acct)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Entities.OperationHeader.OperationHeader header = databaseContext.OperationHeaders.
                    Where(oh => oh.Id == operationHeaderId).
                    FirstOrDefault(); 
                    if (header == null)
                    {
                        return false;
                    }

                    // databaseContext.Exchanges.Add(Exchange.Create(header, direction, appName, appKey, header.OperType));
                    return true;
                }
            });
        }
    }
}
