using Microinvest.CommonLibrary.Enums;
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
    }
}
