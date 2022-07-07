using DataBase.Entities.Partners;
using Microinvest.CommonLibrary.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories.Partners
{
    public interface IPartnerRepository
    {
        /// <summary>
        /// Gets partner from the database by id.
        /// </summary>
        /// <param name="id">Id to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>28.03.2022.</date>
        Task<Partner> GetPartnerByIdAsync(int id);

        /// <summary>
        /// Gets partner from the database by number of a discount card.
        /// </summary>
        /// <param name="discountCardNumber">Number of a discount card to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        Task<Partner> GetPartnerByDiscountCardAsync(string discountCardNumber);

        /// <summary>
        /// Gets partner from the database by tax number, VAT number or e-mail.
        /// </summary>
        /// <param name="key">Key to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        Task<Partner> GetPartnerByKeyAsync(string key);

        /// <summary>
        /// Gets partner from the database by name of partner.
        /// </summary>
        /// <param name="name">Name of partner to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        Task<Partner> GetPartnerByNameAsync(string name);


        /// <summary>
        /// Gets list of partners in according to status of partner.
        /// </summary>
        /// <param name="status">Status of partner.</param>
        /// <returns>List of partners.</returns>
        /// <date>28.03.2022.</date>
        IAsyncEnumerable<Partner> GetParnersAsync(ENomenclatureStatuses status = ENomenclatureStatuses.Active);

        /// <summary>
        /// Gets list of partners in according to path of group, name, tax number, VAT number, e-mail and number of discount card.
        /// </summary>
        /// <param name="groupPath">Path of group.</param>
        /// <param name="searchKey">Key to search by other fields.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        IAsyncEnumerable<Partner> GetParnersAsync(string groupPath, string searchKey);

        /// <summary>
        /// Gets list of partners in according to name, tax number, VAT number, e-mail and number of discount card.
        /// </summary>
        /// <param name="searchKey">Key to search data.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        IAsyncEnumerable<Partner> GetParnersAsync(string searchKey);

        /// <summary>
        /// Gets list of partners in according to id of partner group.
        /// </summary>
        /// <param name="GroupID">Id of partner group to search data.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        IAsyncEnumerable<Partner> GetParnersByGroupIdAsync(int GroupID);

        /// <summary>
        /// Adds new partner to table with partners.
        /// </summary>
        /// <param name="partner">Partner to add to table with partners in the database.</param>
        /// <returns>Returns 0 if partner wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        Task<int> AddPartnerAsync(Partner partner);

        /// <summary>
        /// Updates the partner in the table with partners.
        /// </summary>
        /// <param name="partner">Partner to update in the table of items in the database.</param>
        /// <returns>Returns true if partner was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        Task<bool> UpdatePartnerAsync(Partner partner);

        /// <summary>
        /// Deletes partner by id.
        /// </summary>
        /// <param name="partnerId">Id of partner to delete.</param>
        /// <returns>Returns true if partner was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        Task<bool> DeletePartnerAsync(int partnerId);

        /// <summary>
        /// Checks whether name of partner is duplicated.
        /// </summary>
        /// <param name="partnerName">Name of partner.</param>
        /// <param name="partnerId">Id of partner.</param>
        /// <returns>Returns true if name of partner is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        Task<bool> PartnerNameIsDuplicatedAsync(string partnerName, int partnerId);

        /// <summary>
        /// Checks whether tax number of partner is duplicated.
        /// </summary>
        /// <param name="taxNumber">Tax number of partner.</param>
        /// <param name="partnerId">Id of partner.</param>
        /// <returns>Returns true if tax number of partner is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        Task<bool> PartnerTaxNumberIsDuplicatedAsync(string taxNumber, int partnerId);

        /// <summary>
        /// Checks whether VAT number of partner is duplicated.
        /// </summary>
        /// <param name="vATNumber">VAT number of partner.</param>
        /// <param name="partnerId">Id of partner.</param>
        /// <returns>Returns true if VAT number of partner is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        Task<bool> PartnerVATNumberIsDuplicatedAsync(string vATNumber, int partnerId);

        /// <summary>
        /// Checks whether phone of partner is duplicated.
        /// </summary>
        /// <param name="phone">Phone of partner.</param>
        /// <param name="partnerId">Id of partner.</param>
        /// <returns>Returns true if phone of partner is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        Task<bool> PartnerPhoneIsDuplicatedAsync(string phone, int partnerId);

        /// <summary>
        /// Checks whether e-mail of partner is duplicated.
        /// </summary>
        /// <param name="eMail">E-mail of partner.</param>
        /// <param name="partnerId">Id of partner.</param>
        /// <returns>Returns true if e-mail of partner is duplicated; otherwise returns false.</returns>
        /// <date>06.07.2022.</date>
        Task<bool> PartnerEMailIsDuplicatedAsync(string eMail, int partnerId);
    }
}
