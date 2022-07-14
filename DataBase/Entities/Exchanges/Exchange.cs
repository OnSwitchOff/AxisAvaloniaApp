using DataBase.Enums;
using Microinvest.CommonLibrary.Enums;

namespace DataBase.Entities.Exchanges
{
    public class Exchange : Entity
    {
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
        /// 
        /// </summary>
        /// <date>12.05.2022.</date>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets record of OperationHeader table.
        /// </summary>
        /// <date>12.05.2022.</date>
        public OperationHeader.OperationHeader OperationHeader { get; set; }

        /// <summary>
        /// Gets or sets direction of an exchange.
        /// </summary>
        /// <date>12.05.2022.</date>
        public EExchangeDirections ExchangeType { get; set; }

        /// <summary>
        /// Gets or sets name of app from or for which data import/export.
        /// </summary>
        /// <date>12.05.2022.</date>
        public string AppName { get; set; } = null!;

        /// <summary>
        /// Gets or sets key of app from or for which data import/export.
        /// </summary>
        /// <date>12.05.2022.</date>
        public string AppKey { get; set; } = null!;

        /// <summary>
        /// Gets or sets acct of the imported data.
        /// </summary>
        /// <date>12.05.2022.</date>
        public long Acct { get; set; }

        /// <summary>
        /// Gets or sets operation type of the imported data.
        /// </summary>
        /// <date>12.05.2022.</date>
        public EOperTypes OperType { get; set; }

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
