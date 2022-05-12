using DataBase.Entities.Partners;
using Microinvest.CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.Partners
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public PartnerRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets partner from the database by id.
        /// </summary>
        /// <param name="id">Id to search partner in the database.</param>
        /// <returns>Partner.</returns>
        /// <date>28.03.2022.</date>
        public Task<Partner> GetPartnerByIdAsync(int id)
        {
            return databaseContext.Partners.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets partner from the database by number of a discount card.
        /// </summary>
        /// <param name="discountCardNumber">Number of a discount card to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public Task<Partner> GetPartnerByDiscountCardAsync(string discountCardNumber)
        {
            return databaseContext.Partners.FirstOrDefaultAsync(p => p.DiscountCard.ToLower().Equals(discountCardNumber.ToLower()));
        }

        /// <summary>
        /// Gets partner from the database by tax number, VAT number or e-mail.
        /// </summary>
        /// <param name="key">Key to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public Task<Partner> GetPartnerByKeyAsync(string key)
        {
            return databaseContext.Partners.FirstOrDefaultAsync(p => p.TaxNumber.Equals(key) || p.VATNumber.Equals(key) || p.Email.Equals(key));
        }

        /// <summary>
        /// Gets partner from the database by name of partner.
        /// </summary>
        /// <param name="name">Name of partner to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public Task<Partner> GetPartnerByNameAsync(string name)
        {
            return databaseContext.Partners.FirstOrDefaultAsync(p => p.Company.Equals(name));
        }

        /// <summary>
        /// Gets list of partners.
        /// </summary>
        /// <param name="status">Status of partner.</param>
        /// <returns>List of partners.</returns>
        /// <date>28.03.2022.</date>
        public IAsyncEnumerable<Partner> GetParnersAsync(ENomenclatureStatuses status)
        {
            return databaseContext.Partners.Where(x => x.Status == status).Include(p => p.Group).AsAsyncEnumerable();
        }

        /// <summary>
        /// Gets list of partners in according to path of group, name, tax number, VAT number, e-mail and number of discount card.
        /// </summary>
        /// <param name="groupPath">Path of group.</param>
        /// <param name="searchKey">Key to search by other fields.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        public IAsyncEnumerable<Partner> GetParnersAsync(string groupPath, string searchKey)
        {
            return databaseContext.
                        Partners.
                        Where(p =>
                        (groupPath.Equals("-2") ? 1 == 1 : p.Group.Path.StartsWith(groupPath))
                        &&
                        (string.IsNullOrEmpty(searchKey) ? 1 == 1 :
                        (p.Company.ToLower().Contains(searchKey) ||
                        p.TaxNumber.Contains(searchKey) ||
                        p.VATNumber.Contains(searchKey) ||
                        p.Email.Contains(searchKey) ||
                        p.DiscountCard.Equals(searchKey)
                        ))).
                        AsAsyncEnumerable();
        }

        /// <summary>
        /// Gets list of partners in according to name, tax number, VAT number, e-mail and number of discount card.
        /// </summary>
        /// <param name="searchKey">Key to search data.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        public IAsyncEnumerable<Partner> GetParnersAsync(string searchKey)
        {
            return databaseContext.
                    Partners.
                    Where(p =>
                    p.Company.ToLower().Contains(searchKey.ToLower()) ||
                    p.TaxNumber.Contains(searchKey) ||
                    p.VATNumber.Contains(searchKey) ||
                    p.Email.Contains(searchKey) ||
                    p.DiscountCard.Equals(searchKey)).
                    AsAsyncEnumerable();
        }

        /// <summary>
        /// Gets list of partners in according to id of partner group.
        /// </summary>
        /// <param name="GroupID">Id of partner group to search data.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        public IAsyncEnumerable<Partner> GetParnersAsync(int GroupID)
        {
            return databaseContext.Partners.Where(p => p.Group.Id == GroupID).AsAsyncEnumerable();
        }

        /// <summary>
        /// Adds new partner to table with partners.
        /// </summary>
        /// <param name="partner">Partner to add to table with partners in the database.</param>
        /// <returns>Returns 0 if partner wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        public Task<int> AddPartnerAsync(Partner partner)
        {
            return Task.Run<int>(() =>
            {
                databaseContext.Partners.Add(partner);
                databaseContext.SaveChanges();

                return partner.Id;
            });
        }

        /// <summary>
        /// Updates the partner in the table with partners.
        /// </summary>
        /// <param name="partner">Partner to update in the table of items in the database.</param>
        /// <returns>Returns true if partner was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public Task<bool> UpdatePartnerAsync(Partner partner)
        {
            return Task.Run<bool>(() =>
            {
                databaseContext.Partners.Update(partner);
                return databaseContext.SaveChanges() > 0;
            });
        }

        /// <summary>
        /// Deletes partner by id.
        /// </summary>
        /// <param name="partnerId">Id of partner to delete.</param>
        /// <returns>Returns true if partner was deleted; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public Task<bool> DeletePartnerAsync(int partnerId)
        {
            return Task.Run<bool>(() =>
            {
                Partner partner = databaseContext.Partners.FirstOrDefault(p => p.Id == partnerId);
                if (partner == null)
                {
                    return false;
                }
                else
                {
                    databaseContext.Partners.Remove(partner);
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }
    }
}
