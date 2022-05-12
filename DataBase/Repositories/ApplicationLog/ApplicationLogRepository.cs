namespace DataBase.Repositories.ApplicationLog
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLogRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ApplicationLogRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
