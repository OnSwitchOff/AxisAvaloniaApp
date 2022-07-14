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
        private static object locker = new object();

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
        public async Task<long> GetNextAcctAsync(EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    List<Entities.OperationHeader.OperationHeader> listItems = databaseContext.
                         OperationHeaders.
                         Where(oh => oh.OperType == operType).
                         ToList();

                    if (listItems != null && listItems.Count > 0)
                    {
                        return listItems.Select(oh => oh.Acct).Max() + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            });
        }

        /// <summary>
        /// Get next unique sale number.
        /// </summary>
        /// <param name="fiscalDeviceNumber">Number of a fiscal device for which is needed to find next unique sale number.</param>
        /// <returns>Next unique sale number.</returns>
        /// <date>13.04.2022.</date>
        public async Task<long> GetNextSaleNumberAsync(string fiscalDeviceNumber)
        {
            return await Task.Run(() =>
            {
                lock (locker)
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
            lock (locker)
            {
                return databaseContext.OperationHeaders.FirstOrDefaultAsync(x => x.Id == id);
            }
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
                lock (locker)
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
                }
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
                lock (locker)
                {
                    return GetItemPrice(itemId);
                }
            });
        }

        /// <summary>
        /// Gets price of item.
        /// </summary>
        /// <param name="itemId">Id of item to search price.</param>
        /// <returns>Returns 0 if record is absent; otherwise returns actual price of item.</returns>
        /// <date>13.07.2022.</date>
        public double GetItemPrice(int itemId)
        {
            Entities.OperationHeader.OperationHeader currentPrice = databaseContext.OperationHeaders.
                Where(oh => oh.OperType == EOperTypes.Revaluation).
                Include(h => h.OperationDetails).
                ThenInclude(d => d.Goods).
                Where(delegate (Entities.OperationHeader.OperationHeader h)
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
        }

        /// <summary>
        /// GetOperationHeadersByDates.
        /// </summary>
        /// <returns>Next acc.</returns>
        /// <date>06.07.2022.</date>
        public async Task<List<Entities.OperationHeader.OperationHeader>> GetOperationHeadersByDatesAsync(DateTime from, DateTime to, EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    List<Entities.OperationHeader.OperationHeader> list = databaseContext.OperationHeaders.
                    Where(oh => oh.Date >= from && oh.Date <= to.AddDays(1) && operType == oh.OperType).
                    Include(oh => oh.OperationDetails).ThenInclude(d => d.Goods).ThenInclude(g => g.Vatgroup).
                    Include(oh => oh.OperationDetails).ThenInclude(d => d.Goods).ThenInclude(g => g.Group).
                    Include(oh => oh.Partner).ThenInclude(p => p.Group).
                    Include(oh => oh.Payment).
                    ToList();

                    return list;
                }
            });
        }

        /// <summary>
        /// Gets records in according to parameters.
        /// </summary>
        /// <param name="operType">Type of operation to search data into the database.</param>
        /// <param name="year">Year to search data into the database.</param>
        /// <param name="month">Month to search data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <returns>Returns data in according to parameters to prepare data for export to NAP.</returns>
        /// <date>13.07.2022.</date>
        public async Task<List<Entities.OperationHeader.OperationHeader>> GetRecords(EOperTypes operType, int year, int month, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.OperationHeaders.
                    Where(oh => 
                    oh.OperType == operType &&
                    oh.Date.Year == year &&
                    oh.Date.Month == month).
                    Include(oh => oh.Partner).
                    Include(oh => oh.Payment).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    ToList();
                }
            });
        }

        /// <summary>
        /// Gets records to generate export file for Delta Pro.
        /// </summary>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <returns>Returns data in according to parameters to prepare data for export to NAP.</returns>
        /// <date>13.07.2022.</date>
        public async Task<List<Entities.OperationHeader.OperationHeader>> GetRecordsForDeltaProAsync(DateTime dateFrom, DateTime dateTo, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    // only cash
                    List<Entities.OperationHeader.OperationHeader> records = null;
                    var res = databaseContext.OperationHeaders.
                    Where(oh => 
                    (oh.OperType == EOperTypes.Sale || oh.OperType == EOperTypes.Delivery) &&
                    oh.Date >= dateFrom &&
                    oh.Date <= dateTo.AddDays(1) &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo).
                    Include(oh => oh.Partner).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    SelectMany(oh => oh.OperationDetails, (h, d) => new
                    {
                        h.Id,
                        h.Acct,
                        h.OperType,
                        h.Date,
                        h.Partner,
                        d.Qtty,
                        d.SalePrice,
                        d.PurchasePrice,
                        d.SaleVAT,
                        d.PurchaseVAT,
                        d.Goods.Vatgroup.VATValue,
                    }).
                    AsEnumerable().
                    GroupBy(g => g.VATValue);

                    return records;
                }
            });
        }


        //public double GetLastPriceByGoodId(int goodId)
        //{
        //    try
        //    {
        //        return (double)databaseContext.
        //        OperationHeaders.
        //        Where(oh => oh.OperType == EOperTypes.Revaluation).
        //        Include(oh => oh.OperationDetails).ThenInclude(d => d.Goods).ThenInclude(g => g.Vatgroup).
        //        Where(oh => oh.OperationDetails.Any(od => od.Goods.Id == goodId)).
        //        OrderByDescending(oh => oh.Date).
        //        Select(oh => oh.OperationDetails.Where(od => od.Goods.Id == goodId).First().SalePrice).FirstOrDefault();

        //    }
        //    catch (Exception e)
        //    {
        //        return 0;
        //    }

        //}
    }
}
