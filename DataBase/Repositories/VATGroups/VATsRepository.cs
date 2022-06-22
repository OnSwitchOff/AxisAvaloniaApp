using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Gets group of VAT by Id.
        /// </summary>
        /// <param name="id">Id to find group of VAT.</param>
        /// <returns>Returns group of VAT if group exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public Task<Entities.VATGroups.VATGroup> GetVATGroupByIdAsync(int id)
        {
            return databaseContext.Vatgroups.Where(vg => vg.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds new VAT groups to the database.
        /// </summary>
        /// <param name="vATGroups">List with VAT groups to add to the database.</param>
        /// <returns>Returns 0 if VAT groups were not added to database; otherwise returns count of new records.</returns>
        /// <date>17.06.2022.</date>
        public async Task<int> AddVATGroupsAsync(IList<Entities.VATGroups.VATGroup> vATGroups)
        {
            await databaseContext.Vatgroups.AddRangeAsync(vATGroups);
            return await databaseContext.SaveChangesAsync();
        }
    }
}
