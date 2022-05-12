﻿using Microinvest.CommonLibrary.Enums;
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
        public Task<int> GetNextAcctAsync(EOperTypes operType)
        {
            return Task.Run(() =>
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
        public Task<int> GetNextSaleNumberAsync(string fiscalDeviceNumber)
        {
            return Task.Run(() =>
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
    }
}
