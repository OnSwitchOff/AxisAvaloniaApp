namespace DataBase.Repositories.OperationDetails
{
    public class OperationDetailsRepository : IOperationDetailsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDetailsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public OperationDetailsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
