using System.Threading.Tasks;

namespace DataBase.Repositories.ApplicationLog
{
    public interface IApplicationLogRepository
    {
        /// <summary>
        /// Adds new applicationLog to table with applicationLogs.
        /// </summary>
        /// <param name="applicationLog">applicationLog</param>
        /// <returns>Returns 0 if applicationLog wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>24.06.2022.</date>
        public Task<int> AddApplicationLogAsync(Entities.ApplicationLog.ApplicationLog applicationLog);
    }
}
