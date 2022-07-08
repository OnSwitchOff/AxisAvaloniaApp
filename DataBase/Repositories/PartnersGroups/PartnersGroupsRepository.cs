﻿using DataBase.Entities.PartnersGroups;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.PartnersGroups
{
    public class PartnersGroupsRepository : IPartnersGroupsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersGroupsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public PartnersGroupsRepository(DatabaseContext databaseContext)
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
                PartnersGroup res = databaseContext.PartnersGroups.Where(pg => pg.Id == groupId).FirstOrDefault();
                if (res == null)
                {
                    return "-2";
                }

                return res.Path;
            });
        }

        /// <summary>
        /// Gets group of partners by Id.
        /// </summary>
        /// <param name="id">Id to find group of partners.</param>
        /// <returns>Returns group of partners if group exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public Task<PartnersGroup> GetGroupByIdAsync(int id)
        {
            return databaseContext.PartnersGroups.Where(g => g.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds new group of partners to table with groups of partners.
        /// </summary>
        /// <param name="partnersGroup">Group of partners.</param>
        /// <returns>Returns 0 if group of partners wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        public async Task<int> AddGroupAsync(PartnersGroup partnersGroup)
        {
            return await Task.Run<int>(() =>
            {
                databaseContext.PartnersGroups.Add(partnersGroup);
                databaseContext.SaveChanges();

                return partnersGroup.Id;
            });
        }

        /// <summary>
        /// Updates the group of partners in the table with groups of partners.
        /// </summary>
        /// <param name="partnersGroup">Group of partners.</param>
        /// <returns>Returns true if group of partners was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> UpdateGroupAsync(PartnersGroup partnersGroup)
        {
            return await Task.Run<bool>(() =>
            {
                databaseContext.ChangeTracker.Clear();
                databaseContext.PartnersGroups.Update(partnersGroup);
                return databaseContext.SaveChanges() > 0;
            });
        }

        /// <summary>
        /// Deletes group of partners by id.
        /// </summary>
        /// <param name="groupId">Id of group of partners.</param>
        /// <param name="includeSubGroups">Flag indicating whether should be deleted also subgroups.</param>
        /// <returns>Returns true if group of partners was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> DeleteGroupAsync(int groupId, bool includeSubGroups = true)
        {
            return await Task.Run<bool>(() =>
            {
                PartnersGroup partnersGroup = databaseContext.PartnersGroups.
                Include(pg => pg.Partners).
                FirstOrDefault(i => i.Id == groupId);
                if (partnersGroup == null)
                {
                    return false;
                }
                else
                {
                    if (includeSubGroups)
                    {
                        List<PartnersGroup> subGroups = databaseContext.PartnersGroups.
                        Where(pg => pg.Path.StartsWith(partnersGroup.Path)).
                        Include(pg => pg.Partners).
                        ToList();
                        if (subGroups != null && subGroups.Count > 0)
                        {
                            databaseContext.PartnersGroups.RemoveRange(subGroups);
                        }
                    }

                    databaseContext.PartnersGroups.Remove(partnersGroup);
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }

        /// <summary>
        /// Gets list with groups of partners.
        /// </summary>
        /// <returns>Returns list with groups of partners.</returns>
        /// <date>01.04.2022.</date>
        public Task<List<PartnersGroup>> GetPartnersGroupsAsync()
        {
            return databaseContext.PartnersGroups.ToListAsync();
        }

        /// <summary>
        /// Checks whether name of partners group is duplicated.
        /// </summary>
        /// <param name="partnersGroupName">Name of partners group.</param>
        /// <param name="partnersGroupId">Id of partners group.</param>
        /// <returns>Returns true if name of partners group is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        public async Task<bool> PartnersGroupNameIsDuplicatedAsync(string partnersGroupName, int partnersGroupId)
        {
            return await Task.Run(() =>
            {
                return databaseContext.PartnersGroups.
                Where(i => i.Id != partnersGroupId && i.Name.ToLower().Equals(partnersGroupName.ToLower())).
                FirstOrDefault() != null;
            });
        }
    }
}
