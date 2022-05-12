using System.Collections.Generic;

namespace DataBase.Repositories.PaymentTypes
{
    public class PaymentTypesRepository : IPaymentTypesRepository
    {
        private readonly DatabaseContext databaseContext;

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
            return databaseContext.PaymentTypes.AsAsyncEnumerable();
        }
    }
}
