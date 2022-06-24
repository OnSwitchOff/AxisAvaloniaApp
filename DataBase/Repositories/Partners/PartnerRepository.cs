﻿using DataBase.Entities.Partners;
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
        public async Task<Partner> GetPartnerByIdAsync(int id)
        {
            return await databaseContext.Partners.Include(p => p.Group).FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Gets partner from the database by number of a discount card.
        /// </summary>
        /// <param name="discountCardNumber">Number of a discount card to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public Task<Partner> GetPartnerByDiscountCardAsync(string discountCardNumber)
        {
            return databaseContext.Partners.Include(p => p.Group).FirstOrDefaultAsync(p => p.DiscountCard.ToLower().Equals(discountCardNumber.ToLower()));
        }

        /// <summary>
        /// Gets partner from the database by tax number, VAT number or e-mail.
        /// </summary>
        /// <param name="key">Key to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public async Task<Partner> GetPartnerByKeyAsync(string key)
        {
            return await databaseContext.Partners.Include(p => p.Group).FirstOrDefaultAsync(p => p.TaxNumber.Equals(key) || p.VATNumber.Equals(key) || p.Email.Equals(key));
        }

        /// <summary>
        /// Gets partner from the database by name of partner.
        /// </summary>
        /// <param name="name">Name of partner to search partner in the database.</param>
        /// <returns>Returns Partner if data was searched; otherwise returns null.</returns>
        /// <date>30.03.2022.</date>
        public Task<Partner> GetPartnerByNameAsync(string name)
        {
            return databaseContext.Partners.Include(p => p.Group).FirstOrDefaultAsync(p => p.Company.Equals(name));
        }

        /// <summary>
        /// Gets list of partners.
        /// </summary>
        /// <param name="status">Status of partner.</param>
        /// <returns>List of partners.</returns>
        /// <date>28.03.2022.</date>
        public IAsyncEnumerable<Partner> GetParnersAsync(ENomenclatureStatuses status)
        {
            return databaseContext.Partners.Include(p => p.Group).Where(x => (x.Status == ENomenclatureStatuses.All || x.Status == status)).Include(p => p.Group).AsAsyncEnumerable();
        }

        /// <summary>
        /// Gets list of partners in according to path of group, name, tax number, VAT number, e-mail and number of discount card.
        /// </summary>
        /// <param name="groupPath">Path of group.</param>
        /// <param name="searchKey">Key to search by other fields.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        public async IAsyncEnumerable<Partner> GetParnersAsync(string groupPath, string searchKey)
        {
            foreach (var partner in databaseContext.Partners.Include(p => p.Group))
            {
                if ((groupPath.Equals("-2") ? 1 == 1 : partner.Group.Path.StartsWith(groupPath)) &&
                    (string.IsNullOrEmpty(searchKey) ? 1 == 1 :
                        (partner.Company.ToLower().Contains(searchKey) ||
                        partner.TaxNumber.Contains(searchKey) ||
                        partner.VATNumber.Contains(searchKey) ||
                        partner.Email.Contains(searchKey) ||
                        partner.DiscountCard.Equals(searchKey))))
                {
                    yield return partner;
                }
            }
        }

        /// <summary>
        /// Gets list of partners in according to name, tax number, VAT number, e-mail and number of discount card.
        /// </summary>
        /// <param name="searchKey">Key to search data.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        public async IAsyncEnumerable<Partner> GetParnersAsync(string searchKey)
        {
            foreach(var partner in databaseContext.Partners.Include(p => p.Group))
            {
                if (partner.Company.ToLower().Contains(searchKey.ToLower()) ||
                    partner.TaxNumber.Contains(searchKey) ||
                    partner.VATNumber.Contains(searchKey) ||
                    partner.Email.Contains(searchKey) ||
                    partner.DiscountCard.Equals(searchKey))
                {
                    yield return partner;
                }
            }
        }

        /// <summary>
        /// Gets list of partners in according to id of partner group.
        /// </summary>
        /// <param name="GroupID">Id of partner group to search data.</param>
        /// <returns>List of partners.</returns>
        /// <date>30.03.2022.</date>
        public IAsyncEnumerable<Partner> GetParnersByGroupIdAsync(int GroupID)
        {
            return databaseContext.Partners.Include(p => p.Group).Where(p => p.Group.Id == GroupID).AsAsyncEnumerable();
        }

        /// <summary>
        /// Adds new partner to table with partners.
        /// </summary>
        /// <param name="partner">Partner to add to table with partners in the database.</param>
        /// <returns>Returns 0 if partner wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>31.03.2022.</date>
        public async Task<int> AddPartnerAsync(Partner partner)
        {
            return await Task.Run<int>(() =>
            {
                partner.Group = databaseContext.PartnersGroups.Where(g => g.Id == partner.Group.Id).FirstOrDefault();
                databaseContext.Partners.Add(partner);
                int rec = databaseContext.SaveChanges();

                return partner.Id;
            });
        }

        /// <summary>
        /// Updates the partner in the table with partners.
        /// </summary>
        /// <param name="partner">Partner to update in the table of items in the database.</param>
        /// <returns>Returns true if partner was updated; otherwise returns false.</returns>
        /// <date>31.03.2022.</date>
        public async Task<bool> UpdatePartnerAsync(Partner partner)
        {
            return await Task.Run<bool>(() =>
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
