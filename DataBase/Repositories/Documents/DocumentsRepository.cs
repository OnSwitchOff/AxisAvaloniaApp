namespace DataBase.Repositories.Documents
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public DocumentsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
