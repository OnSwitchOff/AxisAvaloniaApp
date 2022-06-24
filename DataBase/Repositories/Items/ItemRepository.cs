using DataBase.Entities.Items;
using Microinvest.CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.Items
{
    public class ItemRepository : IItemRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ItemRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets item from the database by barcode.
        /// </summary>
        /// <param name="barcode">Barcode to search partner in the database.</param>
        /// <returns>Returns Item if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public async Task<Item> GetItemByBarcodeAsync(string barcode)
        {
            return await databaseContext.Items.Include(i => i.Group).Include(i => i.Vatgroup).FirstOrDefaultAsync(i => i.Barcode.Equals(barcode));
        }

        /// <summary>
        ///  Gets item from the database by id of item.
        /// </summary>
        /// <param name="id">Id to search item in the database.</param>
        /// <returns>Returns Item if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await databaseContext.Items.Include(i => i.Group).Include(i => i.Vatgroup).FirstOrDefaultAsync(i => i.Id == id);
        }

        /// <summary>
        ///  Gets item from the database by name, codes and barcode.
        /// </summary>
        /// <param name="key">Key to search item in the database.</param>
        /// <returns>Returns Item if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public async Task<Item> GetItemByKeyAsync(string key)
        {
            return await databaseContext.
                    Items.
                    Include(i => i.Group).
                    Include(i => i.Vatgroup).
                    FirstOrDefaultAsync(i =>
                    i.Name.Equals(key) ||
                    i.Code.Equals(key) ||
                    i.Barcode.Equals(key) ||
                    i.ItemsCodes.FirstOrDefault(ic => ic.Code.Equals(key)) != null);
        }

        /// <summary>
        /// Gets list of items in according to name, barcode and codes of item.
        /// </summary>
        /// <param name="searchKey">Key to search data.</param>
        /// <returns>List of items.</returns>
        /// <date>30.03.2022.</date>
        public async IAsyncEnumerable<Item> GetItemsAsync(string searchKey)
        {
            foreach (var item in databaseContext.Items.Include(i => i.Group).Include(i => i.Vatgroup))
            {
                if (string.IsNullOrEmpty(searchKey) ? 1 == 1 :
                     (item.Name.ToLower().Contains(searchKey.ToLower()) ||
                     item.Code.Contains(searchKey) ||
                     item.Barcode.Contains(searchKey) ||
                     item.ItemsCodes.Where(ic => ic.Code.Contains(searchKey)).FirstOrDefault() != null))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gets list of items in according to path of group, name, barcode and codes of item.
        /// </summary>
        /// <param name="groupPath">Path of group.</param>
        /// <param name="searchKey">Key to search by other fields.</param>
        /// <returns>List of items.</returns>
        /// <date>30.03.2022.</date>
        public async IAsyncEnumerable<Item> GetItemsAsync(string groupPath, string searchKey)
        {
            foreach (var item in databaseContext.Items.Include(i => i.Group).Include(i => i.Vatgroup))
            {
                if ((groupPath.Equals("-2") ? 1 == 1 : item.Group.Path.StartsWith(groupPath)) &&
                    (string.IsNullOrEmpty(searchKey) ? 1 == 1 :
                    (item.Name.ToLower().Contains(searchKey.ToLower()) ||
                    item.Code.Contains(searchKey) ||
                    item.Barcode.Contains(searchKey) ||
                    item.ItemsCodes.Where(ic => ic.Code.Contains(searchKey)).FirstOrDefault() != null)))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gets list of items in according to id of item group.
        /// </summary>
        /// <param name="groupId">Id of item group to search data.</param>
        /// <returns>List of items.</returns>
        /// <date>30.03.2022.</date>
        public IAsyncEnumerable<Item> GetItemsByGroupIdAsync(int groupId)
        {
            return databaseContext.
                    Items.
                    Where(i => i.Group.Id == groupId).
                    Include(i => i.Group).
                    Include(i => i.Vatgroup).
                    AsAsyncEnumerable();
        }

        /// <summary>
        /// Gets list of items in according to status of item.
        /// </summary>
        /// <param name="status">Status of item.</param>
        /// <returns>List of items.</returns>
        /// <date>17.06.2022.</date>
        public IAsyncEnumerable<Item> GetItemsAsync(ENomenclatureStatuses status = ENomenclatureStatuses.Active)
        {
            return databaseContext.
                Items.
                Where(i => (i.Status == ENomenclatureStatuses.All || i.Status == status)).
                Include(i => i.Group).
                Include(i => i.Vatgroup).
                AsAsyncEnumerable();
        }

        /// <summary>
        /// Adds new item to table of items.
        /// </summary>
        /// <param name="item">Item to add to table of items in the database.</param>
        /// <returns>Returns 0 if item wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        public async Task<int> AddItemAsync(Item item)
        {
            return await Task.Run<int>(() =>
            {
                item.Group = databaseContext.ItemsGroups.Where(g => g.Id == item.Group.Id).FirstOrDefault();
                item.Vatgroup = databaseContext.Vatgroups.Where(vg => vg.Id == item.Vatgroup.Id).FirstOrDefault();
                databaseContext.Items.AddAsync(item);
                databaseContext.SaveChangesAsync();

                return item.Id;
            });
        }

        /// <summary>
        /// Updates the item in the table of items.
        /// </summary>
        /// <param name="item">Item to update in the table of items in the database.</param>
        /// <returns>Returns true if item was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> UpdateItemAsync(Item item)
        {
            return await Task.Run<bool>(() =>
            {
                databaseContext.Items.Update(item);
                return databaseContext.SaveChanges() > 0;
            });
        }

        /// <summary>
        /// Deletes item by id.
        /// </summary>
        /// <param name="itemId">Id of item to delete.</param>
        /// <returns>Returns true if item was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> DeleteItemAsync(int itemId)
        {
            return await Task.Run<bool>(() =>
            {
                Item item = databaseContext.Items.FirstOrDefault(i => i.Id == itemId);
                if (item == null)
                {
                    return false;
                }
                else
                {
                    databaseContext.Items.Remove(item);
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }

        /// <summary>
        /// Gets list of existing measures.
        /// </summary>
        /// <returns>Returns list of existing measures.</returns>
        /// <date>31.03.2022.</date>
        public async Task<List<string>> GetMeasuresAsync()
        {
            return await Task.Run<List<string>>(() =>
            {
                List<string> list = new List<string>();
                list.AddRange(databaseContext.Items.Select(i => i.Measure).Distinct().ToList());
                list.AddRange(databaseContext.ItemsCodes.Select(ic => ic.Measure).Distinct().ToList());

                return list.Distinct().ToList();
            });
        }
    }
}
