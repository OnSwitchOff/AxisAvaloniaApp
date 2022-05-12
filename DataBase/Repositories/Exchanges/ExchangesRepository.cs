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
    }
}
