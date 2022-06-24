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

        /// <summary>
        /// Gets type of payment by index of payment.
        /// </summary>
        /// <param name="paymentType">Index of payment.</param>
        /// <returns>PaymentType.</returns>
        /// <date>23.06.2022.</date>
        System.Threading.Tasks.Task<Entities.PaymentTypes.PaymentType> GetPaymentTypeByIndexAsync(Microinvest.CommonLibrary.Enums.EPaymentTypes paymentType);

        /// <summary>
        /// Adds new types of payment to the database.
        /// </summary>
        /// <param name="paymentTypes">List with types of payment to add to the database.</param>
        /// <returns>Returns 0 if types of payment were not added to database; otherwise returns count of new records.</returns>
        /// <date>24.06.2022.</date>
        System.Threading.Tasks.Task<int> AddPaymentTypesAsync(IList<Entities.PaymentTypes.PaymentType> paymentTypes);
    }
}
