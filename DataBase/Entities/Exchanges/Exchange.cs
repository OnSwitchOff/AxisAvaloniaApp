using DataBase.Enums;
using Microinvest.CommonLibrary.Enums;

namespace DataBase.Entities.Exchanges
{
    public class Exchange : Entity
    {
        public int Id { get; set; }
        public OperationHeader.OperationHeader OperationHeader { get; set; }
        public EExchangeDirections ExchangeType { get; set; }
        public string AppName { get; set; } = null!;
        public string AppKey { get; set; } = null!;
        public long Acct { get; set; }
        public EOperTypes OperType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exchange"/> class.
        /// </summary>
        internal Exchange() : this(null, EExchangeDirections.Export, "", "", 0, EOperTypes.Sale)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exchange"/> class.
        /// </summary>
        /// <param name="operHeader">The operation that is the base for the current record.</param>
        /// <param name="exchangeType">Type of an exchange.</param>
        /// <param name="appName">The name of the application to which the data was exported or from which the data was imported.</param>
        /// <param name="appKey">The unique key of the application to which the data was exported or from which the data was imported.</param>
        /// <param name="acct">The number of the operation that was imported or exported.</param>
        /// <param name="operType">The type of the operation that was imported or exported.</param>
        private Exchange(OperationHeader.OperationHeader operHeader, EExchangeDirections exchangeType, string appName, string appKey, long acct, EOperTypes operType)
        {
            this.OperationHeader = operHeader;
            this.ExchangeType = exchangeType;
            this.AppName = appName;
            this.AppKey = appKey;
            this.Acct = acct;
            this.OperType = operType;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Exchange"/> class.
        /// </summary>
        /// <param name="operHeader">The operation that is the base for the current record.</param>
        /// <param name="exchangeType">Type of an exchange.</param>
        /// <param name="appName">The name of the application to which the data was exported or from which the data was imported.</param>
        /// <param name="appKey">The unique key of the application to which the data was exported or from which the data was imported.</param>
        /// <param name="acct">The number of the operation that was imported or exported.</param>
        /// <param name="operType">The type of the operation that was imported or exported.</param>
        /// <returns>Returns <see cref="Exchange"/> class if parameters are correct.</returns>
        public static Exchange Create(OperationHeader.OperationHeader operHeader, EExchangeDirections exchangeType, string appName, string appKey, long acct, EOperTypes operType)
        {
            // check rule

            return new Exchange(operHeader, exchangeType, appName, appKey, acct, operType);
        }
    }
}
