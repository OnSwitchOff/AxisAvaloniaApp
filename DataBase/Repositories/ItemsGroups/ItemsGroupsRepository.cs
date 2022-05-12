using DataBase.Entities.ItemsGroups;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.ItemsGroups
{
    public class ItemsGroupsRepository : IItemsGroupsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsGroupsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ItemsGroupsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets path of group by id of group.
        /// </summary>
        /// <param name="groupId">Id of group.</param>
        /// <returns>Returns "-2" if group is absent; otherwise returns path of group.</returns>
        /// <date>31.03.2022.</date>
        public Task<string> GetPathByIdAsync(int groupId)
        {
            return Task.Run<string>(() =>
            {

                ItemsGroup res = databaseContext.ItemsGroups.Where(pg => pg.Id == groupId).FirstOrDefault();
                if (res == null)
                {
                    return "-2";
                }

                return res.Path;
            });
        }

        /// <summary>
        /// Adds new group of items to table with groups of items.
        /// </summary>
        /// <param name="itemsGroup">Group of items.</param>
        /// <returns>Returns 0 if group of items wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        public Task<int> AddGroupAsync(ItemsGroup itemsGroup)
        {
            databaseContext.ItemsGroups.Add(itemsGroup);
            databaseContext.SaveChangesAsync();

            return Task.FromResult(itemsGroup.Id);
        }

        /// <summary>
        /// Updates the group of items in the table with groups of items.
        /// </summary>
        /// <param name="itemsGroup">Group of items.</param>
        /// <returns>Returns true if group of items was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public Task<bool> UpdateGroupAsync(ItemsGroup itemsGroup)
        {
            return Task.Run<bool>(() =>
            {
                databaseContext.ItemsGroups.Update(itemsGroup);
                return databaseContext.SaveChanges() > 0;
            });
        }

        /// <summary>
        /// Deletes group of items by id.
        /// </summary>
        /// <param name="groupId">Id of group of item.</param>
        /// <returns>Returns true if group of items was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public Task<bool> DeleteGroupAsync(int groupId)
        {
            return Task.Run<bool>(() =>
            {
                ItemsGroup itemsGroup = databaseContext.ItemsGroups.FirstOrDefault(i => i.Id == groupId);
                if (itemsGroup == null)
                {
                    return false;
                }
                else
                {
                    databaseContext.ItemsGroups.Remove(itemsGroup);
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }

        /// <summary>
        /// Gets list with groups of items.
        /// </summary>
        /// <returns>Returns list with groups of items.</returns>
        /// <date>01.04.2022.</date>
        public Task<List<ItemsGroup>> GetItemsGroupsAsync()
        {
            return databaseContext.ItemsGroups.ToListAsync();
        }
    }
}
