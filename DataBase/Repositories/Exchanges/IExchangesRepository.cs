using DataBase.Entities.Exchanges;
using Microinvest.ExchangeDataService.Enums;
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
        Task<(bool isExist, int id, int acct)> CheckRecordAsync(EExchanges exchangeType, string appName, string appKey);

        /// <summary>
        /// Gets record in according to parameters.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns Exchange object if record exist; otherwise returns null.</returns>
        /// <date>16.06.2022.</date>
        Task<Exchange> GetRecordAsync(EExchanges exchangeType, string appName, string appKey);
    }
}
