using Microinvest.CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.OperationHeader
{
    public class OperationHeaderRepository : IOperationHeaderRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationHeaderRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public OperationHeaderRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Get next acct to the concrete operation.
        /// </summary>
        /// <param name="operType">Operation type for which is needed to find next account number.</param>
        /// <returns>Next acc.</returns>
        /// <date>13.04.2022.</date>
        public async Task<int> GetNextAcctAsync(EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                List<Entities.OperationHeader.OperationHeader> listItems = databaseContext.
                     OperationHeaders.
                     Where(oh => oh.OperType == operType).
                     ToList();

                if (listItems != null && listItems.Count > 0)
                {
                    return listItems.Max(oh => oh.Acct) + 1;
                }
                else
                {
                    return 1;
                }

            });
        }

        /// <summary>
        /// Get next unique sale number.
        /// </summary>
        /// <param name="fiscalDeviceNumber">Number of a fiscal device for which is needed to find next unique sale number.</param>
        /// <returns>Next unique sale number.</returns>
        /// <date>13.04.2022.</date>
        public async Task<int> GetNextSaleNumberAsync(string fiscalDeviceNumber)
        {
            return await Task.Run(() =>
            {
                List<Entities.OperationHeader.OperationHeader> listItems = databaseContext.
                    OperationHeaders.
                    Where(oh => oh.OperType == EOperTypes.Sale && oh.USN.Equals(fiscalDeviceNumber)).
                    ToList();

                if (listItems != null && listItems.Count > 0)
                {
                    return listItems.Max(oh => oh.EcrreceiptNumber);
                }
                else
                {
                    return 1;
                }

            });
        }

        /// <summary>
        /// Gets OperationHeader from the database by id.
        /// </summary>
        /// <param name="id">Id to search OperationHeader in the database.</param>
        /// <returns>OperationHeader</returns>
        /// <date>24.06.2022.</date>
        public Task<Entities.OperationHeader.OperationHeader> GetOperationHeaderByIdAsync(int id)
        {
            return databaseContext.OperationHeaders.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Adds new record to OperationHeader table.
        /// </summary>
        /// <param name="record">Data to add.</param>
        /// <returns>Returns 0 if record wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>23.06.2022.</date>
        public async Task<int> AddNewRecordAsync(Entities.OperationHeader.OperationHeader record)
        {
            return await Task.Run(() =>
            {
                foreach (var detail in record.OperationDetails)
                {
                    detail.Goods = databaseContext.Items.Where(i => i.Id == detail.Goods.Id).FirstOrDefault();
                }

                if (record.Partner != null)
                {
                    record.Partner = databaseContext.Partners.Where(p => p.Id == record.Partner.Id).FirstOrDefault();
                }

                if (record.Payment != null)
                {
                    record.Payment = databaseContext.PaymentTypes.Where(pt => pt.Id == record.Payment.Id).FirstOrDefault();
                }

                databaseContext.OperationHeaders.Add(record);
                databaseContext.SaveChanges();

                return record.Id;
            });
        }

        /// <summary>
        /// Gets price of item.
        /// </summary>
        /// <param name="itemId">Id of item to search price.</param>
        /// <returns>Returns 0 if record is absent; otherwise returns actual price of item.</returns>
        /// <date>05.07.2022.</date>
        public async Task<double> GetItemPriceAsync(int itemId)
        {
            return await Task.Run(() => 
            {
                Entities.OperationHeader.OperationHeader currentPrice = databaseContext.OperationHeaders.
                Where(oh => oh.OperType == EOperTypes.Revaluation).
                Include(h => h.OperationDetails).
                ThenInclude(d => d.Goods).
                Where(delegate(Entities.OperationHeader.OperationHeader h)
                {
                    foreach (var detail in h.OperationDetails)
                    {
                        if (detail.Goods.Id == itemId)
                        {
                            return true;
                        }
                    }

                    return false;
                }).MaxBy(h => h.Date);

                if (currentPrice != null)
                {
                    return (double)currentPrice.OperationDetails.Where(d => d.Goods.Id == itemId).Select(i => i.SalePrice).FirstOrDefault();
                }
                return 0;
            });
        }


        /// <summary>
        /// GetOperationHeadersByDates.
        /// </summary>
        /// <returns>Next acc.</returns>
        /// <date>06.07.2022.</date>
        public async Task<List<Entities.OperationHeader.OperationHeader>> GetOperationHeadersByDatesAsync(DateTime from, DateTime to)
        {
            return await Task.Run(async () =>
            {
                return databaseContext.
                     OperationHeaders.
                     Where(oh => oh.Date >= from && oh.Date <= to.AddDays(1)).
                     Include(oh => oh.OperationDetails).
                     Include(oh => oh.Partner).
                     ToList();
            });
        }
    }
}
