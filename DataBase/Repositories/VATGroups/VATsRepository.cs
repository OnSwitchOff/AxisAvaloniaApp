using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.VATGroups
{
    public class VATsRepository : IVATsRepository
    {
        private readonly DatabaseContext databaseContext;
        private static object locker = new object();

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
            lock (locker)
            {
                return databaseContext.Vatgroups.AsAsyncEnumerable();
            }
        }

        /// <summary>
        /// Gets group of VAT by Id.
        /// </summary>
        /// <param name="id">Id to find group of VAT.</param>
        /// <returns>Returns group of VAT if group exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public async Task<Entities.VATGroups.VATGroup> GetVATGroupByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Vatgroups.Where(vg => vg.Id == id).FirstOrDefault();
                }
            });            
        }

        /// <summary>
        /// Adds new VAT groups to the database.
        /// </summary>
        /// <param name="vATGroups">List with VAT groups to add to the database.</param>
        /// <returns>Returns 0 if VAT groups were not added to database; otherwise returns count of new records.</returns>
        /// <date>17.06.2022.</date>
        public async Task<int> AddVATGroupsAsync(IList<Entities.VATGroups.VATGroup> vATGroups)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    databaseContext.Vatgroups.AddRange(vATGroups);
                    return databaseContext.SaveChanges();
                }
            });            
        }

        /// <summary>
        /// Gets group of VAT by key.
        /// </summary>
        /// <param name="name">Name of VAT group.</param>
        /// <param name="value">Value of VAT.</param>
        /// <returns></returns>
        /// <date>21.07.2022.</date>
        public async Task<Entities.VATGroups.VATGroup> GetVATGroupByKeyAsync(string name, decimal value)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Entities.VATGroups.VATGroup group = databaseContext.Vatgroups.
                    FirstOrDefault(v => v.Name == name && v.VATValue == value);

                    if (group == null)
                    {
                        return databaseContext.Vatgroups.
                        FirstOrDefault(group => group.VATValue == value);
                    }
                    else
                    {
                        return group;
                    }
                }
            });
        }
    }
}
