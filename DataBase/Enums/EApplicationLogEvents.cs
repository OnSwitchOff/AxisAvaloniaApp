namespace DataBase.Enums
{
    /// <summary>
    /// App actions
    /// </summary>
    public enum EApplicationLogEvents
    {
        /// <summary>
        /// Enter or exit app
        /// </summary>
        ApplicationStart = 0,

        /// <summary>
        /// Create product
        /// </summary>
        CreateItem = 10,

        /// <summary>
        /// Edit product
        /// </summary>
        EditItem = 11,

        /// <summary>
        /// Delete product
        /// </summary>
        DeleteItem = 12,

        /// <summary>
        /// Create partner
        /// </summary>
        CreatePartner = 21,

        /// <summary>
        /// Edit partner
        /// </summary>
        EditPartner = 22,

        /// <summary>
        /// Delete partner
        /// </summary>
        DeletePartner = 23,

        /// <summary>
        /// Lock selected partner
        /// </summary>
        LockPartner = 24,

        /// <summary>
        /// UnlockSelectedPartner
        /// </summary>
        UnlockPartner = 25,

        /// <summary>
        /// Create product group
        /// </summary>
        CreateItemGroup = 31,

        /// <summary>
        /// Edit product group
        /// </summary>
        EditItemGroup = 32,

        /// <summary>
        /// Delete product group
        /// </summary>
        DeleteItemGroup = 33,

        /// <summary>
        /// Create partner group
        /// </summary>
        CreatePartnerGroup = 34,

        /// <summary>
        /// Edit partner group
        /// </summary>
        EditPartnerGroup = 35,

        /// <summary>
        /// Delete partner group
        /// </summary>
        DeletePartnerGroup = 36,

        /// <summary>
        /// Flag indicating whether partner is selected
        /// </summary>
        SelectPartnerToOperation = 41,

        /// <summary>
        /// Add product to operation
        /// </summary>
        AddItemToOperation = 42,

        /// <summary>
        /// Delete row from a table
        /// </summary>
        DeleteRowInOperation = 43,

        /// <summary>
        /// Save operation
        /// </summary>
        SaveOperation = 44,

        /// <summary>
        /// Enter a payment
        /// </summary>
        EnterPayment = 45,

        /// <summary>
        /// Print receipt
        /// </summary>
        PrintReceipt = 46,

        /// <summary>
        /// Error during printer communication
        /// </summary>
        ErrorInCommunicationWithPrinter = 47,

        /// <summary>
        /// Issue invoice
        /// </summary>
        IssueInvoice = 48,

        /// <summary>
        /// Refund operation parameters
        /// </summary>
        RefundParameters = 49,

        /// <summary>
        /// Close operation
        /// </summary>
        CloseOperation = 59,

        /// <summary>
        /// Reprint operation data
        /// </summary>
        ReprintOperation = 58,

        /// <summary>
        /// Create report
        /// </summary>
        GenerateReport,

        /// <summary>
        /// Change app settings
        /// </summary>
        ChangeSettings,

        /// <summary>
        /// Licensing
        /// </summary>
        Licensing,
    }
}
