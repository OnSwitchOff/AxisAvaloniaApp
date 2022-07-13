using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.PaymentTypes
{
    public class PaymentTypesRepository : IPaymentTypesRepository
    {
        private readonly DatabaseContext databaseContext;
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypesRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public PaymentTypesRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets list with types of payments.
        /// </summary>
        /// <returns>Returns list with types of payments.</returns>
        /// <date>05.05.2022.</date>
        public IAsyncEnumerable<Entities.PaymentTypes.PaymentType> GetPaymentTypes()
        {
            lock (locker)
            {
                return databaseContext.PaymentTypes.AsAsyncEnumerable();
            }
        }

        /// <summary>
        /// Gets type of payment by index of payment.
        /// </summary>
        /// <param name="paymentType">Index of payment.</param>
        /// <returns>PaymentType.</returns>
        /// <date>23.06.2022.</date>
        public async Task<Entities.PaymentTypes.PaymentType> GetPaymentTypeByIndexAsync(Microinvest.CommonLibrary.Enums.EPaymentTypes paymentType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.PaymentTypes.FirstOrDefault(p => p.PaymentIndex == paymentType);
                }
            });            
        }

        /// <summary>
        /// Adds new types of payment to the database.
        /// </summary>
        /// <param name="paymentTypes">List with types of payment to add to the database.</param>
        /// <returns>Returns 0 if types of payment were not added to database; otherwise returns count of new records.</returns>
        /// <date>24.06.2022.</date>
        public async Task<int> AddPaymentTypesAsync(IList<Entities.PaymentTypes.PaymentType> paymentTypes)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    databaseContext.AddRange(paymentTypes);
                    return databaseContext.SaveChanges();
                }
            });
        }
    }
}
