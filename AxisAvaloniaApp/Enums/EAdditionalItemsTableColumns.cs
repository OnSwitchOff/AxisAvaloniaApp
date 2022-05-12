namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Additional columns in the items table.
    /// </summary>
    public enum EAdditionalItemsTableColumns : int
    {
        /// <summary>
        /// Column named "Barcode".
        /// </summary>
        Barcode = 1 << 1,

        /// <summary>
        /// Column named "Measure".
        /// </summary>
        Measure = 1 << 2,

        /// <summary>
        /// Column named "VATGroup".
        /// </summary>
        VATGroup = 1 << 3,
    }
}
