using System.Threading.Tasks;

namespace DataBase.Repositories.OperationDetails
{
    public class OperationDetailsRepository : IOperationDetailsRepository
    {
        private readonly DatabaseContext databaseContext;
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDetailsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public OperationDetailsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Adds new record to OperationDetail table.
        /// </summary>
        /// <param name="record">Data to add.</param>
        /// <returns>Returns 0 if record wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>23.06.2022.</date>
        public async Task<int> AddNewRecord(Entities.OperationDetails.OperationDetail record)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    databaseContext.OperationDetails.Add(record);
                    databaseContext.SaveChanges();

                    return record.Id;
                }
            });
        }
    }
}
