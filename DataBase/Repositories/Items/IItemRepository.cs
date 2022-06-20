using DataBase.Entities.Items;
using Microinvest.CommonLibrary.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories.Items
{
    public interface IItemRepository
    {
        /// <summary>
        /// Gets item from the database by barcode.
        /// </summary>
        /// <param name="barcode">Barcode to search partner in the database.</param>
        /// <returns>Returns Item if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        Task<Item> GetItemByBarcodeAsync(string barcode);

        /// <summary>
        ///  Gets item from the database by id of item.
        /// </summary>
        /// <param name="id">Id to search item in the database.</param>
        /// <returns>Returns Item if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        Task<Item> GetItemByIdAsync(int id);

        /// <summary>
        ///  Gets item from the database by name and code.
        /// </summary>
        /// <param name="key">Key to search item in the database.</param>
        /// <returns>Returns Item if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        Task<Item> GetItemByKeyAsync(string key);

        /// <summary>
        /// Gets list of items in according to name, barcode and codes of item.
        /// </summary>
        /// <param name="searchKey">Key to search data.</param>
        /// <returns>List of items.</returns>
        /// <date>30.03.2022.</date>
        IAsyncEnumerable<Item> GetItemsAsync(string searchKey);

        /// <summary>
        /// Gets list of items in according to path of group, name, barcode and codes of item.
        /// </summary>
        /// <param name="groupPath">Path of group.</param>
        /// <param name="searchKey">Key to search by other fields.</param>
        /// <returns>List of items.</returns>
        /// <date>30.03.2022.</date>
        IAsyncEnumerable<Item> GetItemsAsync(string groupPath, string searchKey);

        /// <summary>
        /// Gets list of items in according to id of item group.
        /// </summary>
        /// <param name="groupId">Id of item group to search data.</param>
        /// <returns>List of items.</returns>
        /// <date>30.03.2022.</date>
        IAsyncEnumerable<Item> GetItemsByGroupIdAsync(int groupId);

        /// <summary>
        /// Gets list of items in according to status of item.
        /// </summary>
        /// <param name="status">Status of item.</param>
        /// <returns>List of items.</returns>
        /// <date>17.06.2022.</date>
        IAsyncEnumerable<Item> GetItemsAsync(ENomenclatureStatuses status = ENomenclatureStatuses.Active);

        /// <summary>
        /// Adds new item to table of items.
        /// </summary>
        /// <param name="item">Item to add to table of items in the database.</param>
        /// <returns>Returns 0 if item wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        Task<int> AddItemAsync(Item item);

        /// <summary>
        /// Updates the item in the table of items.
        /// </summary>
        /// <param name="item">Item to update in the table of items in the database.</param>
        /// <returns>Returns true if item was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        Task<bool> UpdateItemAsync(Item item);

        /// <summary>
        /// Deletes item by id.
        /// </summary>
        /// <param name="itemId">Id of item to delete.</param>
        /// <returns>Returns true if item was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        Task<bool> DeleteItemAsync(int itemId);

        /// <summary>
        /// Gets list of existing measures.
        /// </summary>
        /// <returns>Returns list of existing measures.</returns>
        /// <date>31.03.2022.</date>
        Task<List<string>> GetMeasuresAsync();
    }
}
