namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Additional columns in the document table.
    /// </summary>
    public enum EAdditionalDocumentColumns : int
    {
        /// <summary>
        /// Column named "City".
        /// </summary>
        City = 1 << 1,

        /// <summary>
        /// Column named "Address".
        /// </summary>
        Address = 1 << 2,

        /// <summary>
        /// Column named "Phone".
        /// </summary>
        Phone = 1 << 3,
    }
}
