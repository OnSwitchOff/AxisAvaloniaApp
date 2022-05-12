using System.Collections.Generic;

namespace DataBase.Repositories.VATGroups
{
    public class VATsRepository : IVATsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VATsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public VATsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets list with groups of VATs.
        /// </summary>
        /// <returns>Returns list with groups of VATs.</returns>
        /// <date>05.05.2022.</date>
        public IAsyncEnumerable<Entities.VATGroups.VATGroup> GetVATGroupsAsync()
        {

            return databaseContext.Vatgroups.AsAsyncEnumerable();
        }
    }
}
