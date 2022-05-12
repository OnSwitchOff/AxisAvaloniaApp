namespace DataBase.Repositories.ItemsCodes
{
    public class ItemsCodesRepository : IItemsCodesRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsCodesRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ItemsCodesRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
    }
}
