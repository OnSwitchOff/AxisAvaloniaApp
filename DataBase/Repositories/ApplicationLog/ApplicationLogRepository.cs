using System.Threading.Tasks;

namespace DataBase.Repositories.ApplicationLog
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly DatabaseContext databaseContext;
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLogRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ApplicationLogRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }


        /// <summary>
        /// Adds new applicationLog to table with applicationLogs.
        /// </summary>
        /// <param name="applicationLog">applicationLog</param>
        /// <returns>Returns 0 if applicationLog wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>24.06.2022.</date>
        public Task<int> AddApplicationLogAsync(Entities.ApplicationLog.ApplicationLog applicationLog)
        {
            return Task.Run<int>(() =>
            {
                lock (locker)
                {
                    databaseContext.ApplicationLogs.Add(applicationLog);
                    databaseContext.SaveChanges();

                    return applicationLog.Id;
                }
            });
        }
    }
}
