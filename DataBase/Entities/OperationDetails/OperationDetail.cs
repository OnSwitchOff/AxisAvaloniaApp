namespace DataBase.Entities.OperationDetails
{
    public class OperationDetail : Entity
    {
        public int Id { get; set; }
        public OperationHeader.OperationHeader OperationHeader { get; set; }
        public Items.Item Goods { get; set; }
        public decimal Qtty { get; set; }
        public int Sign { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PurchaseVAT { get; set; }
        public decimal SaleVAT { get; set; }
        public int SrcId { get; set; }
        public string Note { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDetail"/> class.
        /// </summary>
        internal OperationDetail() : this(null, null, 0, 0, 0, 0, 0, 0, 0, "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDetail"/> class.
        /// </summary>
        /// <param name="operationHeader">Record in the OperationHeader table.</param>
        /// <param name="goods">Goods included in the operation.</param>
        /// <param name="qtty">Quantity of goods included in the operation.</param>
        /// <param name="sign">Sign of the operation.</param>
        /// <param name="purchasePrice">Purchase price.</param>
        /// <param name="salePrice">Sale price.</param>
        /// <param name="purchaseVat">Vat value by the purchase price.</param>
        /// <param name="saleVat">Vat value by the sale price.</param>
        /// <param name="srcId">The record id that is the base for the current record (for the refund operation).</param>
        /// <param name="note">Note of the record.</param>
        private OperationDetail(OperationHeader.OperationHeader operationHeader, Items.Item goods, decimal qtty, int sign, decimal purchasePrice, decimal salePrice, decimal purchaseVat, decimal saleVat, int srcId, string note)
        {
            this.OperationHeader = operationHeader;
            this.Goods = goods;
            this.Qtty = qtty;
            this.Sign = sign;
            this.PurchasePrice = purchasePrice;
            this.SalePrice = salePrice;
            this.PurchaseVAT = purchaseVat;
            this.SaleVAT = saleVat;
            this.SrcId = srcId;
            this.Note = note;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OperationDetail"/> class.
        /// </summary>
        /// <param name="operationHeader">Record in the OperationHeader table.</param>
        /// <param name="goods">Goods included in the operation.</param>
        /// <param name="qtty">Quantity of goods included in the operation.</param>
        /// <param name="sign">Sign of the operation.</param>
        /// <param name="salePrice">Sale price.</param>
        /// <param name="saleVat">Vat value by the sale price.</param>
        /// <param name="srcId">The record id that is the base for the current record (for the refund operation).</param>
        /// <param name="note">Note of the record.</param>
        /// <returns>Returns <see cref="OperationDetail"/> class if parameters are correct.</returns>
        public static OperationDetail Create(OperationHeader.OperationHeader operationHeader, Items.Item goods, decimal qtty, int sign, decimal salePrice, decimal saleVat, int srcId, string note = "")
        {
            // check number

            return new OperationDetail(operationHeader, goods, qtty, sign, 0, salePrice, 0, saleVat, srcId, note);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OperationDetail"/> class.
        /// </summary>
        /// <param name="operationHeader">Record in the OperationHeader table.</param>
        /// <param name="goods">Goods included in the operation.</param>
        /// <param name="qtty">Quantity of goods included in the operation.</param>
        /// <param name="sign">Sign of the operation.</param>
        /// <param name="salePrice">Sale price.</param>
        /// <param name="saleVat">Vat value by the sale price.</param>
        /// <param name="note">Note of the record.</param>
        /// <returns>Returns <see cref="OperationDetail"/> class if parameters are correct.</returns>
        public static OperationDetail Create(OperationHeader.OperationHeader operationHeader, Items.Item goods, decimal qtty, int sign, decimal salePrice, decimal saleVat, string note = "")
        {
            // check number

            return new OperationDetail(operationHeader, goods, qtty, sign, 0, salePrice, 0, saleVat, 0, note);
        }
    }
}
