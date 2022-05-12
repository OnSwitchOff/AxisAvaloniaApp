namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Additional columns in the sale table.
    /// </summary>
    public enum EAdditionalSaleTableColumns : int
    {
        /// <summary>
        /// Column named "Code".
        /// </summary>
        Code = 1 << 1,

        /// <summary>
        /// Column named "Barcode".
        /// </summary>
        Barcode = 1 << 2,

        /// <summary>
        /// Column named "Note".
        /// </summary>
        Note = 1 << 3,
    }
}
