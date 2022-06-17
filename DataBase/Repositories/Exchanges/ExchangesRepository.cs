using DataBase.Entities.Exchanges;
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
        public async Task<(bool isExist, int id, int acct)> CheckRecordAsync(EExchanges exchangeType, string appName, string appKey)
        {
            Exchange record = await databaseContext.Exchanges.
                Where(r => r.ExchangeType == exchangeType && r.AppName.Equals(appName) && r.AppKey.Equals(appKey)).
                FirstOrDefaultAsync();

            if (record != null)
            {
                return (true, record.Id, record.Acct);
            }
            else
            {
                return (false, 0, 0);
            }
        }

        /// <summary>
        /// Gets record in according to parameters.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns Exchange object if record exist; otherwise returns null.</returns>
        /// <date>16.06.2022.</date>
        public async Task<Exchange> GetRecordAsync(EExchanges exchangeType, string appName, string appKey)
        {
            return await databaseContext.Exchanges.
                Where(r => r.ExchangeType == exchangeType && r.AppName.Equals(appName) && r.AppKey.Equals(appKey)).
                FirstOrDefaultAsync();
        }
    }
}
