using DataBase.Entities.PartnersGroups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories.PartnersGroups
{
    public interface IPartnersGroupsRepository
    {
        /// <summary>
        /// Gets path of group by id of group.
        /// </summary>
        /// <param name="groupId">Id of group.</param>
        /// <returns>Returns "-2" if group is absent; otherwise returns path of group.</returns>
        /// <date>31.03.2022.</date>
        Task<string> GetPathByIdAsync(int groupId);

        /// <summary>
        /// Gets group of partners by Id.
        /// </summary>
        /// <param name="id">Id to find group of partners.</param>
        /// <returns>Returns group of items if partners exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        Task<PartnersGroup> GetGroupByIdAsync(int id);

        /// <summary>
        /// Adds new group of partners to table with groups of partners.
        /// </summary>
        /// <param name="partnersGroup">Group of partners.</param>
        /// <returns>Returns 0 if group of partners wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        Task<int> AddGroupAsync(PartnersGroup partnersGroup);

        /// <summary>
        /// Updates the group of partners in the table with groups of partners.
        /// </summary>
        /// <param name="partnersGroup">Group of partners.</param>
        /// <returns>Returns true if group of partners was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        Task<bool> UpdateGroupAsync(PartnersGroup partnersGroup);

        /// <summary>
        /// Deletes group of partners by id.
        /// </summary>
        /// <param name="groupId">Id of group of partners.</param>
        /// <param name="includeSubGroups">Flag indicating whether should be deleted also subgroups.</param>
        /// <returns>Returns true if group of partners was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        Task<bool> DeleteGroupAsync(int groupId, bool includeSubGroups = true);

        /// <summary>
        /// Gets list with groups of partners.
        /// </summary>
        /// <returns>Returns list with groups of partners.</returns>
        /// <date>01.04.2022.</date>
        Task<List<PartnersGroup>> GetPartnersGroupsAsync();

        /// <summary>
        /// Checks whether name of partners group is duplicated.
        /// </summary>
        /// <param name="partnersGroupName">Name of partners group.</param>
        /// <param name="partnersGroupId">Id of partners group.</param>
        /// <returns>Returns true if name of partners group is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        Task<bool> PartnersGroupNameIsDuplicatedAsync(string partnersGroupName, int partnersGroupId);
    }
}
