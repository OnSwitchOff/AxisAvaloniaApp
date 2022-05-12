using Microinvest.CommonLibrary.Enums;

namespace DataBase.Entities.PaymentTypes
{
    public class PaymentType : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public EPaymentTypes PaymentIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentType"/> class.
        /// </summary>
        internal PaymentType() : this("", EPaymentTypes.Unknown)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentType"/> class.
        /// </summary>
        /// <param name="name">Name of type of payment.</param>
        /// <param name="paymentIndex">Index of type of payment.</param>
        private PaymentType(string name, EPaymentTypes paymentIndex)
        {
            this.Name = name;
            this.PaymentIndex = paymentIndex;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PaymentType"/> class.
        /// </summary>
        /// <param name="name">Name of type of payment.</param>
        /// <param name="paymentIndex">Index of type of payment.</param>
        /// <returns>Returns <see cref="PaymentType"/> class if parameters are correct.</returns>
        public static PaymentType Create(string name, EPaymentTypes paymentIndex)
        {
            // check rule

            return new PaymentType(name, paymentIndex);
        }
    }
}
