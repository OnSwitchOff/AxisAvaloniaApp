using System.Collections.Generic;

namespace DataBase.Repositories.VATGroups
{
    public interface IVATsRepository
    {
        /// <summary>
        /// Gets list with groups of VATs.
        /// </summary>
        /// <returns>Returns list with groups of VATs.</returns>
        /// <date>05.05.2022.</date>
        IAsyncEnumerable<Entities.VATGroups.VATGroup> GetVATGroupsAsync();

        /// <summary>
        /// Adds new VAT groups to the database.
        /// </summary>
        /// <param name="vATGroups">List with VAT groups to add to the database.</param>
        /// <returns>Returns 0 if VAT groups were not added to database; otherwise returns count of new records.</returns>
        /// <date>17.06.2022.</date>
        System.Threading.Tasks.Task<int> AddVATGroupsAsync(IList<Entities.VATGroups.VATGroup> vATGroups);
    }
}
