using System.Collections.Generic;

namespace DataBase.Repositories.PaymentTypes
{
    public interface IPaymentTypesRepository
    {
        /// <summary>
        /// Gets list with types of payments.
        /// </summary>
        /// <returns>Returns list with types of payments.</returns>
        /// <date>05.05.2022.</date>
        IAsyncEnumerable<Entities.PaymentTypes.PaymentType> GetPaymentTypes();
    }
}
