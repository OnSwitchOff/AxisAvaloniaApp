namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Additional columns in the partners table.
    /// </summary>
    public enum EAdditionalPartnersTableColumns : int
    {
        /// <summary>
        /// Column named "Number of discount card".
        /// </summary>
        DiscountCard = 1 << 1,

        /// <summary>
        /// Column named "Principal".
        /// </summary>
        Principal = 1 << 2,

        /// <summary>
        /// Column named "Phone".
        /// </summary>
        Phone = 1 << 3,

        /// <summary>
        /// Column named "Number of VAT".
        /// </summary>
        VATNumber = 1 << 4,

        /// <summary>
        /// Column named "City".
        /// </summary>
        City = 1 << 5,

        /// <summary>
        /// Column named "Address".
        /// </summary>
        Address = 1 << 6,

        /// <summary>
        /// Column named "Tax number".
        /// </summary>
        TaxNumber = 1 << 7,

        /// <summary>
        /// Column named "E-mail".
        /// </summary>
        EMail = 1 << 8,
    }
}
