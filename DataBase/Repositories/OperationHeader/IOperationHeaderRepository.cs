using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories.OperationHeader
{
    public interface IOperationHeaderRepository
    {
        /// <summary>
        /// Get next acct to the concrete operation.
        /// </summary>
        /// <param name="operType">Operation type for which is needed to find next account number.</param>
        /// <returns>Next acc.</returns>
        /// <date>13.04.2022.</date>
        Task<int> GetNextAcctAsync(EOperTypes operType);

        /// <summary>
        /// Get next unique sale number.
        /// </summary>
        /// <param name="fiscalDeviceNumber">Number of a fiscal device for which is needed to find next unique sale number.</param>
        /// <returns>Next unique sale number.</returns>
        /// <date>13.04.2022.</date>
        Task<int> GetNextSaleNumberAsync(string fiscalDeviceNumber);

        /// <summary>
        /// Gets OperationHeader from the database by id.
        /// </summary>
        /// <param name="id">Id to search OperationHeader in the database.</param>
        /// <returns>OperationHeader</returns>
        /// <date>24.06.2022.</date>
        Task<Entities.OperationHeader.OperationHeader> GetOperationHeaderByIdAsync(int id);

        /// <summary>
        /// Adds new record to OperationHeader table.
        /// </summary>
        /// <param name="record">Data to add.</param>
        /// <returns>Returns 0 if record wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>23.06.2022.</date>
        Task<int> AddNewRecordAsync(Entities.OperationHeader.OperationHeader record);

        /// <summary>
        /// Gets price of item.
        /// </summary>
        /// <param name="itemId">Id of item to search price.</param>
        /// <returns>Returns 0 if record is absent; otherwise returns actual price of item.</returns>
        /// <date>05.07.2022.</date>
        Task<double> GetItemPriceAsync(int itemId);
    }
}
