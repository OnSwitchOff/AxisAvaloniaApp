using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Gets group of VAT by Id.
        /// </summary>
        /// <param name="id">Id to find group of VAT.</param>
        /// <returns>Returns group of VAT if group exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        Task<Entities.VATGroups.VATGroup> GetVATGroupByIdAsync(int id);

        /// <summary>
        /// Adds new VAT groups to the database.
        /// </summary>
        /// <param name="vATGroups">List with VAT groups to add to the database.</param>
        /// <returns>Returns 0 if VAT groups were not added to database; otherwise returns count of new records.</returns>
        /// <date>17.06.2022.</date>
        Task<int> AddVATGroupsAsync(IList<Entities.VATGroups.VATGroup> vATGroups);
    }
}
