﻿using DataBase.Entities.ItemsGroups;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.ItemsGroups
{
    public class ItemsGroupsRepository : IItemsGroupsRepository
    {
        private readonly DatabaseContext databaseContext;
        private static object locker = new object();

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
        public async Task<string> GetPathByIdAsync(int groupId)
        {
            return await Task.Run<string>(() =>
            {
                lock (locker)
                {
                    ItemsGroup res = databaseContext.ItemsGroups.Where(pg => pg.Id == groupId).FirstOrDefault();
                    if (res == null)
                    {
                        return "-2";
                    }

                    return res.Path;
                }
            });
        }

        /// <summary>
        /// Gets group of items by Id.
        /// </summary>
        /// <param name="id">Id to find group of items.</param>
        /// <returns>Returns group of items if group exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public async Task<ItemsGroup> GetGroupByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.ItemsGroups.
                    Where(g => g.Id == id).
                    FirstOrDefault();
                }
            });
        }

        /// <summary>
        /// Adds new group of items to table with groups of items.
        /// </summary>
        /// <param name="itemsGroup">Group of items.</param>
        /// <returns>Returns 0 if group of items wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        public async Task<int> AddGroupAsync(ItemsGroup itemsGroup)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    databaseContext.ItemsGroups.Add(itemsGroup);
                    databaseContext.SaveChanges();

                    return itemsGroup.Id;
                }
            });            
        }

        /// <summary>
        /// Updates the group of items in the table with groups of items.
        /// </summary>
        /// <param name="itemsGroup">Group of items.</param>
        /// <returns>Returns true if group of items was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> UpdateGroupAsync(ItemsGroup itemsGroup)
        {
            return await Task.Run<bool>(() =>
            {
                lock (locker)
                {
                    databaseContext.ChangeTracker.Clear();
                    databaseContext.ItemsGroups.Update(itemsGroup);
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }

        /// <summary>
        /// Deletes group of items by id.
        /// </summary>
        /// <param name="groupId">Id of group of item.</param>
        /// <param name="includeSubGroups">Flag indicating whether should be deleted also subgroups.</param>
        /// <returns>Returns true if group of items was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> DeleteGroupAsync(int groupId, bool includeSubGroups = true)
        {
            return await Task.Run<bool>(() =>
            {
                lock (locker)
                {
                    ItemsGroup itemsGroup = databaseContext.ItemsGroups.
                    Include(ig => ig.Items).
                    FirstOrDefault(i => i.Id == groupId);
                    if (itemsGroup == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (includeSubGroups)
                        {
                            List<ItemsGroup> subGroups = databaseContext.ItemsGroups.
                            Where(ig => ig.Path.StartsWith(itemsGroup.Path)).
                            Include(ig => ig.Items).
                            ToList();
                            if (subGroups != null && subGroups.Count > 0)
                            {
                                databaseContext.ItemsGroups.RemoveRange(subGroups);
                            }
                        }

                        databaseContext.ItemsGroups.Remove(itemsGroup);
                        return databaseContext.SaveChanges() > 0;
                    }
                }
            });
        }

        /// <summary>
        /// Gets list with groups of items.
        /// </summary>
        /// <returns>Returns list with groups of items.</returns>
        /// <date>01.04.2022.</date>
        public async Task<List<ItemsGroup>> GetItemsGroupsAsync()
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.ItemsGroups.ToList();
                }
            });
        }

        /// <summary>
        /// Checks whether name of items group is duplicated.
        /// </summary>
        /// <param name="itemsGroupName">Name of items group.</param>
        /// <param name="itemsGroupId">Id of items group.</param>
        /// <returns>Returns true if name of items group is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        public async Task<bool> ItemsGroupNameIsDuplicatedAsync(string itemsGroupName, int itemsGroupId)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.ItemsGroups.
                    Where(i => i.Id != itemsGroupId && i.Name.ToLower().Equals(itemsGroupName.ToLower())).
                    FirstOrDefault() != null;
                }
            });
        }
    }
}
