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
    }
}
