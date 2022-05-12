using Microinvest.CommonLibrary.Enums;
using System;

namespace DataBase.Entities.Store
{
    public class Store : Entity
    {
        public int Id { get; set; }
        public EOperTypes OperType { get; set; }
        public OperationDetails.OperationDetail Src { get; set; }
        public Items.Item Item { get; set; }
        public decimal Qtty { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Store"/> class.
        /// </summary>
        internal Store() : this(EOperTypes.Sale, null, null, 0, 0, DateTime.Now)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Store"/> class.
        /// </summary>
        /// <param name="operType">The type of operation that is the base to change count of item.</param>
        /// <param name="src">The record that is the base to change count of item.</param>
        /// <param name="item">Item data.</param>
        /// <param name="qtty">Quantity of item.</param>
        /// <param name="price">Price of item.</param>
        /// <param name="date">Date to add new record.</param>
        private Store(EOperTypes operType, OperationDetails.OperationDetail src, Items.Item item, decimal qtty, decimal price, DateTime date)
        {
            this.OperType = operType;
            this.Src = src;
            this.Item = item;
            this.Qtty = qtty;
            this.Price = price;
            this.Date = date;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Store"/> class.
        /// </summary>
        /// <param name="operType">The type of operation that is the base to change count of item.</param>
        /// <param name="src">The record that is the base to change count of item.</param>
        /// <param name="item">Item data.</param>
        /// <param name="qtty">Quantity of item.</param>
        /// <param name="price">Price of item.</param>
        /// <param name="date">Date to add new record.</param>
        /// <returns>Returns <see cref="Store"/> class if parameters are correct.</returns>
        public static Store Create(EOperTypes operType, OperationDetails.OperationDetail src, Items.Item item, decimal qtty, decimal price, DateTime date)
        {
            // check rule

            return new Store(operType, src, item, qtty, price, date);
        }
    }
}
